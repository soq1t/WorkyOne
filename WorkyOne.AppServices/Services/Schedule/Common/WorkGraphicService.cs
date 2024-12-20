﻿using System.Text;
using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Users;
using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Contracts.Enums.Result;
using WorkyOne.Contracts.Repositories.Result;
using WorkyOne.Contracts.Services.Common;
using WorkyOne.Contracts.Services.CreateModels.Schedule.Common;
using WorkyOne.Contracts.Services.GetRequests.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Domain.Requests.Schedule.Common;
using WorkyOne.Domain.Specifications.AccesFilters.Common;
using WorkyOne.Domain.Specifications.AccesFilters.Schedule.Common;
using WorkyOne.Domain.Specifications.Base;
using WorkyOne.Domain.Specifications.Common;

namespace WorkyOne.AppServices.Services.Schedule.Common
{
    /// <summary>
    /// Сервис работы с рабочим графиком
    /// </summary>
    public sealed class WorkGraphicService : IWorkGraphicService
    {
        private readonly IDailyInfosRepository _dailyInfosRepo;
        private readonly ISchedulesRepository _schedulesRepo;
        private readonly IMapper _mapper;
        private readonly IUserAccessInfoProvider _userAccessInfoProvider;

        private UserAccessInfo _accessInfo;
        private ScheduleAccessFilter _scheduleAccessFilter;
        private DailyInfoAccessFilter _dailyInfoAccessFilter;

        public WorkGraphicService(
            IDailyInfosRepository dailyInfosRepo,
            ISchedulesRepository schedulesRepo,
            IMapper mapper,
            IUserAccessInfoProvider userAccessInfoProvider
        )
        {
            _dailyInfosRepo = dailyInfosRepo;
            _schedulesRepo = schedulesRepo;
            _mapper = mapper;
            _userAccessInfoProvider = userAccessInfoProvider;

            InitFiltersAsync().Wait();
        }

        public async Task<RepositoryResult> ClearAsync(
            string scheduleId,
            CancellationToken cancellation = default
        )
        {
            var filter = new Specification<DailyInfoEntity>(x => x.ScheduleId == scheduleId).And(
                _dailyInfoAccessFilter
            );

            var result = await _dailyInfosRepo.DeleteByConditionAsync(filter, cancellation);

            if (result.IsSucceed)
            {
                return RepositoryResult.Ok(
                    $"График для расписания (ID: {scheduleId}) был полностью очищен!"
                );
            }
            else
            {
                return result;
            }
        }

        public async Task<RepositoryResult> CreateAsync(
            WorkGraphicModel model,
            CancellationToken cancellation = default
        )
        {
            var filter = new EntityIdFilter<ScheduleEntity>(model.ScheduleId).And(
                _scheduleAccessFilter
            );

            var request = new ScheduleRequest(filter)
            {
                IncludeShifts = true,
                IncludeTemplate = true,
            };

            var schedule = await _schedulesRepo.GetAsync(request, cancellation);

            if (schedule == null)
            {
                return RepositoryResult.Error(
                    ResultType.NotFound,
                    model.ScheduleId,
                    nameof(ScheduleEntity)
                );
            }

            var endDate = model.EndDate.Value;
            var startDate = model.StartDate.Value;

            var infos = new List<DailyInfoEntity>(endDate.DayNumber - startDate.DayNumber + 1);

            for (DateOnly date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (cancellation.IsCancellationRequested)
                {
                    return RepositoryResult.CancellationRequested();
                }
                DailyInfoEntity info = GetDailyInfo(schedule, date);
                infos.Add(info);
            }

            await ClearAsync(schedule.Id, cancellation);

            var result = await _dailyInfosRepo.CreateManyAsync(infos, cancellation);

            if (result.IsSucceed)
            {
                await _dailyInfosRepo.SaveChangesAsync(cancellation);

                var message = new StringBuilder(
                    $"Рабочий график для расписания (ID: {model.ScheduleId}) был успешно создан!\n"
                );
                message.AppendLine(
                    $"Промежуток созданного графика: {model.StartDate} - {model.EndDate}"
                );
                message.AppendLine($"Затронуто дней: {infos.Count}");

                return RepositoryResult.Ok(message.ToString());
            }
            return result;
        }

        public async Task<List<DailyInfoDto>> GetGraphicAsync(
            PaginatedWorkGraphicRequest request,
            CancellationToken cancellation = default
        )
        {
            var scheduleId = request.ScheduleId;
            var startDate = request.StartDate.Value;
            var endDate = request.EndDate.Value;

            var filter = new Specification<DailyInfoEntity>(x =>
                x.ScheduleId == scheduleId && x.Date >= startDate && x.Date <= endDate
            ).And(_dailyInfoAccessFilter);

            var entities = await _dailyInfosRepo.GetManyAsync(
                new PaginatedRequest<DailyInfoEntity>(filter, request.PageIndex, request.Amount),
                cancellation
            );

            var dtos = _mapper.Map<List<DailyInfoDto>>(entities);

            return dtos.OrderBy(x => x.Date).ToList();
        }

        private static DailyInfoEntity GetDailyInfo(ScheduleEntity schedule, DateOnly date)
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
                    Name = "Свободный день",
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

        private static DailyInfoEntity GetFromTemplate(TemplateEntity template, DateOnly date)
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

        private async Task InitFiltersAsync(CancellationToken cancellation = default)
        {
            if (_accessInfo == null)
            {
                _accessInfo = await _userAccessInfoProvider.GetCurrentAsync(cancellation);

                _scheduleAccessFilter = new ScheduleAccessFilter(_accessInfo);
                _dailyInfoAccessFilter = new DailyInfoAccessFilter(_accessInfo);
            }
        }
    }
}
