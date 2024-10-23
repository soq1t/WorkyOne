using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Users;
using WorkyOne.AppServices.Interfaces.Services;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Utilities;
using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Requests.Schedule.Common;
using WorkyOne.Domain.Requests.Users;

namespace WorkyOne.AppServices.Services.Schedule.Common
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

        private readonly IEntityUpdateUtility _entityUpdateUtility;

        public ScheduleService(
            ISchedulesRepository schedulesRepository,
            IDailyInfosRepository dailyInfosRepository,
            IUserDatasRepository userDatasRepository,
            IMapper mapper,
            IDateTimeService dateService,
            IEntityUpdateUtility entityUpdateUtility
        )
        {
            _schedulesRepository = schedulesRepository;
            _dailyInfosRepository = dailyInfosRepository;
            _userDatasRepository = userDatasRepository;
            _mapper = mapper;
            _dateService = dateService;
            _entityUpdateUtility = entityUpdateUtility;
        }

        public async Task<bool> ClearDailyAsync(
            string scheduleId,
            CancellationToken cancellation = default
        )
        {
            var result = await _dailyInfosRepository.DeleteByConditionAsync(e =>
                e.ScheduleId == scheduleId
            );

            if (result.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<string?> CreateScheduleAsync(
            string scheduleName,
            string userDataId,
            CancellationToken cancellation = default
        )
        {
            var userData = await _userDatasRepository.GetAsync(
                new UserDataRequest { EntityId = userDataId },
                cancellation
            );

            if (userData == null)
            {
                return null;
            }

            if (cancellation.IsCancellationRequested)
            {
                return null;
            }
            var schedule = new ScheduleEntity() { UserDataId = userDataId, Name = scheduleName, };

            var result = await _schedulesRepository.CreateAsync(schedule, cancellation);

            if (result.IsSuccess)
            {
                return schedule.Id;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteSchedulesAsync(
            List<string> schedulesIds,
            CancellationToken cancellation = default
        )
        {
            var result = await _schedulesRepository.DeleteManyAsync(schedulesIds, cancellation);

            if (cancellation.IsCancellationRequested)
            {
                return false;
            }
            return result.IsSuccess;
        }

        public async Task<List<DailyInfoDto>> GenerateDailyAsync(
            string scheduleId,
            DateOnly startDate,
            DateOnly endDate,
            CancellationToken cancellation = default
        )
        {
            var schedule = await _schedulesRepository.GetAsync(
                new ScheduleRequest
                {
                    EntityId = scheduleId,
                    IncludeTemplate = true,
                    IncludeShifts = true,
                },
                cancellation
            );

            if (schedule == null)
            {
                return new List<DailyInfoDto>();
            }

            var infos = new List<DailyInfoEntity>(endDate.DayNumber - startDate.DayNumber + 1);

            for (DateOnly date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (cancellation.IsCancellationRequested)
                {
                    return new List<DailyInfoDto>();
                }
                DailyInfoEntity info = GetDailyInfo(schedule, date);
                infos.Add(info);
            }

            await _dailyInfosRepository.CreateManyAsync(infos, cancellation);

            if (cancellation.IsCancellationRequested)
            {
                return new List<DailyInfoDto>();
            }

            return _mapper.Map<List<DailyInfoDto>>(infos);
        }

        public async Task<ScheduleDto?> GetAsync(
            string scheduleId,
            CancellationToken cancellation = default
        )
        {
            var request = new ScheduleRequest(scheduleId, true);

            var item = await _schedulesRepository.GetAsync(request, cancellation);

            if (item == null)
            {
                return null;
            }
            else
            {
                return _mapper.Map<ScheduleDto>(item);
            }
        }

        public async Task<List<ScheduleDto>> GetByUserDataAsync(
            string userDataId,
            CancellationToken cancellation = default
        )
        {
            var request = new PaginatedScheduleRequest(userDataId);

            var entities = await _schedulesRepository.GetManyAsync(request, cancellation);

            var dtos = _mapper.Map<List<ScheduleDto>>(entities);
            return dtos;
        }

        public async Task<List<DailyInfoDto>> GetDailyAsync(
            string scheduleId,
            DateOnly startDate,
            DateOnly endDate,
            CancellationToken cancellation = default
        )
        {
            if (endDate < startDate)
            {
                return [];
            }

            var request = new PaginatedDailyInfoRequest(startDate, endDate, scheduleId);

            var entities = await _dailyInfosRepository.GetManyAsync(request, cancellation);
            var dtos = _mapper.Map<List<DailyInfoDto>>(entities);
            return dtos;
        }

        public async Task<List<ScheduleDto>> GetManyAsync(
            int pageIndex,
            int amount,
            bool includeFullData = false,
            CancellationToken cancellation = default
        )
        {
            var request = new PaginatedScheduleRequest
            {
                PageIndex = pageIndex,
                Amount = amount,
                IncludeShifts = includeFullData,
                IncludeTemplate = includeFullData,
            };

            var entities = await _schedulesRepository.GetManyAsync(request, cancellation);

            var dtos = _mapper.Map<List<ScheduleDto>>(entities);

            return dtos;
        }

        public async Task<bool> UpdateScheduleAsync(
            ScheduleDto scheduleDto,
            CancellationToken cancellation = default
        )
        {
            ScheduleEntity source = _mapper.Map<ScheduleEntity>(scheduleDto);

            var target = await _schedulesRepository.GetAsync(
                new ScheduleRequest { EntityId = source.Id }
            );

            if (target == null)
            {
                return false;
            }

            _entityUpdateUtility.Update(target, source);

            var result = _schedulesRepository.Update(target);

            if (result.IsSuccess)
            {
                await _schedulesRepository.SaveChangesAsync(cancellation);
            }

            return result.IsSuccess;
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
                position = position == 0 ? sequenceLength - 1 : position - 1;

                var sequence = template.Sequences.OrderByDescending(s => s.Position).ToList();
                var shift = sequence[position].Shift;

                return DailyInfoEntity.CreateFromShiftEntity(shift, date);
            }
        }
    }
}
