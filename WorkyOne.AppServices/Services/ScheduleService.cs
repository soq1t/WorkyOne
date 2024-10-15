using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Users;
using WorkyOne.AppServices.Interfaces.Services;
using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Contracts.Requests.Common;
using WorkyOne.Contracts.Requests.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Common;

namespace WorkyOne.AppServices.Services
{
    /// <summary>
    /// Сервис по работе с расписанием
    /// </summary>
    public sealed class ScheduleService : IScheduleService
    {
        private readonly ISchedulesRepository _schedulesRepository;
        private readonly IDailyInfosRepository _dailyInfosRepository;
        private readonly IUserDatasRepository _userDatasRepository;
        private readonly IMapper _mapper;
        private readonly IDateTimeService _dateService;

        public ScheduleService(
            ISchedulesRepository schedulesRepository,
            IDailyInfosRepository dailyInfosRepository,
            IUserDatasRepository userDatasRepository,
            IMapper mapper,
            IDateTimeService dateService
        )
        {
            _schedulesRepository = schedulesRepository;
            _dailyInfosRepository = dailyInfosRepository;
            _userDatasRepository = userDatasRepository;
            _mapper = mapper;
            _dateService = dateService;
        }

        public async Task ClearDailyAsync(string scheduleId)
        {
            var deletedIds = (
                await _dailyInfosRepository.GetByScheduleIdAsync(
                    new DailyInfoRequest { ScheduleId = scheduleId }
                )
            )
                .Select(x => x.Id)
                .ToList();

            if (deletedIds.Any())
            {
                await _dailyInfosRepository.DeleteManyAsync(deletedIds);
            }
        }

        public async Task<string?> CreateScheduleAsync(string scheduleName, string userDataId)
        {
            var userData = await _userDatasRepository.GetAsync(
                new UserDataRequest { Id = userDataId }
            );

            if (userData == null)
            {
                return null;
            }

            var schedule = new ScheduleEntity() { UserDataId = userDataId, Name = scheduleName, };

            await _schedulesRepository.CreateAsync(schedule);

            return schedule.Id;
        }

        public Task DeleteSchedulesAsync(List<string> schedulesIds)
        {
            return _schedulesRepository.DeleteManyAsync(schedulesIds);
        }

        public async Task GenerateDailyAsync(
            string scheduleId,
            DateOnly startDate,
            DateOnly endDate
        )
        {
            var infos = new List<DailyInfoEntity>();

            var schedule = await _schedulesRepository.GetAsync(
                new ScheduleRequest
                {
                    Id = scheduleId,
                    IncludeTemplate = true,
                    IncludeDatedShifts = true,
                    IncludePeriodicShifts = true
                }
            );

            if (schedule == null)
            {
                return;
            }

            for (DateOnly date = startDate; date <= endDate; date.AddDays(1))
            {
                DailyInfoEntity info = GetDailyInfo(schedule, date);
                infos.Add(info);
            }

            await _dailyInfosRepository.CreateManyAsync(infos);
        }

        public async Task<ICollection<DailyInfoDto>> GetDailyAsync(string scheduleId)
        {
            var infoEntities = await _dailyInfosRepository.GetByScheduleIdAsync(
                new DailyInfoRequest { ScheduleId = scheduleId }
            );

            List<DailyInfoDto> infoDtos = _mapper.Map<List<DailyInfoDto>>(infoEntities);

            return infoDtos;
        }

        public Task UpdateScheduleAsync(ScheduleDto scheduleDto)
        {
            ScheduleEntity schedule = _mapper.Map<ScheduleEntity>(scheduleDto);

            return _schedulesRepository.UpdateAsync(schedule);
        }

        private DailyInfoEntity GetDailyInfo(ScheduleEntity schedule, DateOnly date)
        {
            var datedShift = schedule.DatedShifts.FirstOrDefault(d => d.Date == date);

            if (datedShift != null)
            {
                var info = DailyInfoEntity.CreateFromShiftEntity(datedShift, date);
                info.ScheduleId = schedule.Id;
                info.Schedule = schedule;
                return info;
            }

            var periodicShift = schedule.PeriodicShifts.FirstOrDefault(d =>
                d.StartDate <= date && d.EndDate >= date
            );

            if (periodicShift != null)
            {
                var info = DailyInfoEntity.CreateFromShiftEntity(periodicShift, date);
                info.ScheduleId = schedule.Id;
                info.Schedule = schedule;
                return info;
            }

            if (schedule.Template == null)
            {
                return new DailyInfoEntity
                {
                    IsBusyDay = false,
                    Date = date,
                    Schedule = schedule,
                    ScheduleId = schedule.Id
                };
            }
            else
            {
                var info = GetFromTemplate(schedule.Template, date);
                info.ScheduleId = schedule.Id;
                info.Schedule = schedule;
                return info;
            }
        }

        private DailyInfoEntity GetFromTemplate(TemplateEntity template, DateOnly date)
        {
            if (template.Sequences.Count == 0)
            {
                return new DailyInfoEntity { IsBusyDay = false, Date = date };
            }

            int sequenceLength = template.Sequences.Count;
            var passedDays = date.DayNumber - template.StartDate.DayNumber;
            passedDays = Math.Abs(passedDays);
            var position = passedDays % sequenceLength;

            if (date >= template.StartDate)
            {
                var sequence = template.Sequences.OrderBy(s => s.Position).ToList();
                var shift = sequence[position].Shift;

                return DailyInfoEntity.CreateFromShiftEntity(shift, date);
            }
            else
            {
                position = position == 0 ? sequenceLength - 1 : position--;

                var sequence = template.Sequences.OrderByDescending(s => s.Position).ToList();
                var shift = sequence[position].Shift;

                return DailyInfoEntity.CreateFromShiftEntity(shift, date);
            }
        }
    }
}
