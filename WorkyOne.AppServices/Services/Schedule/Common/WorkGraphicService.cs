using System.Text;
using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services.Users;
using WorkyOne.AppServices.Interfaces.Stores;
using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Contracts.Enums.Result;
using WorkyOne.Contracts.Repositories.Result;
using WorkyOne.Contracts.Services.CreateModels.Schedule.Common;
using WorkyOne.Contracts.Services.GetRequests.Schedule.Common;
using WorkyOne.Domain.Entities.Abstractions.Shifts;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Domain.Requests.Schedule.Common;
using WorkyOne.Domain.Specifications.AccesFilters.Abstractions;
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

        private readonly IAccessFiltersStore _accessFiltersStore;

        private AccessFilter<ScheduleEntity> _scheduleAccessFilter =>
            _accessFiltersStore.GetFilter<ScheduleEntity>();

        private AccessFilter<DailyInfoEntity> _dailyInfoAccessFilter =>
            _accessFiltersStore.GetFilter<DailyInfoEntity>();

        public WorkGraphicService(
            IDailyInfosRepository dailyInfosRepo,
            ISchedulesRepository schedulesRepo,
            IMapper mapper,
            IUserAccessInfoProvider userAccessInfoProvider,
            IAccessFiltersStore accessFiltersStore
        )
        {
            _dailyInfosRepo = dailyInfosRepo;
            _schedulesRepo = schedulesRepo;
            _mapper = mapper;
            _userAccessInfoProvider = userAccessInfoProvider;
            _accessFiltersStore = accessFiltersStore;
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

        public async Task<RepositoryResult> RecalculateAsync(
            string scheduleId,
            CancellationToken cancellation = default
        )
        {
            var filter = new Specification<DailyInfoEntity>(x => x.ScheduleId == scheduleId).And(
                _dailyInfoAccessFilter
            );

            var daysAmount = await _dailyInfosRepo.GetAmountAsync(
                new EntityRequest<DailyInfoEntity>(filter),
                cancellation
            );

            if (daysAmount == 0)
            {
                return RepositoryResult.Error("Не найден график для указанного расписания");
            }

            var first = await _dailyInfosRepo.GetManyAsync(
                new PaginatedRequest<DailyInfoEntity>(filter, 1, 1),
                cancellation
            );

            var last = await _dailyInfosRepo.GetManyAsync(
                new PaginatedRequest<DailyInfoEntity>(filter, daysAmount, 1),
                cancellation
            );

            var startDate = first.Single().Date;
            var endDate = last.Single().Date;

            return await CreateAsync(
                new WorkGraphicModel()
                {
                    EndDate = endDate,
                    StartDate = startDate,
                    ScheduleId = scheduleId
                },
                cancellation
            );
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
                return GetFromShift(datedShift.Shift, schedule, date);
            }

            var periodicShift = schedule.PeriodicShifts.FirstOrDefault(d =>
                d.StartDate <= date && d.EndDate >= date
            );

            if (periodicShift != null)
            {
                return GetFromShift(periodicShift.Shift, schedule, date);
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
                return GetFromTemplate(schedule.Template, schedule, date);
            }
        }

        private static DailyInfoEntity GetFromTemplate(
            TemplateEntity template,
            ScheduleEntity schedule,
            DateOnly date
        )
        {
            if (template.Shifts.Count == 0)
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

            int sequenceLength = template.Shifts.Count;
            var passedDays = date.DayNumber - template.StartDate.DayNumber;
            passedDays = Math.Abs(passedDays);
            var position = passedDays % sequenceLength;

            if (date >= template.StartDate)
            {
                var sequence = template.Shifts.OrderBy(s => s.Position).ToList();
                var shift = sequence[position].Shift;

                return GetFromShift(shift, schedule, date);
            }
            else
            {
                position = position == 0 ? sequenceLength - 1 : position - 1;

                var sequence = template.Shifts.OrderByDescending(s => s.Position).ToList();
                var shift = sequence[position].Shift;

                return GetFromShift(shift, schedule, date);
            }
        }

        private static DailyInfoEntity GetFromShift(
            ShiftEntity shift,
            ScheduleEntity schedule,
            DateOnly date
        )
        {
            var result = new DailyInfoEntity
            {
                IsBusyDay = shift.Beginning.HasValue && shift.Ending.HasValue,
                Name = shift.Name,
                ColorCode = shift.ColorCode,
                Schedule = schedule,
                ScheduleId = schedule.Id,
                Date = date
            };

            if (result.IsBusyDay)
            {
                result.ShiftProlongation = shift.Duration();
                result.Beginning = shift.Beginning;
                result.Ending = shift.Ending;
            }

            return result;
        }
    }
}
