using System.Diagnostics;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WorkyOne.AppServices.Interfaces.Repositories.Context;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services.Auth;
using WorkyOne.AppServices.Interfaces.Services.Common;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services.Users;
using WorkyOne.AppServices.Interfaces.Stores;
using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Contracts.Enums.Result;
using WorkyOne.Contracts.Options.Schedules;
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
        private readonly IOptions<WorkGraphicOptions> _options;

        private readonly IDailyInfosRepository _dailyInfosRepo;
        private readonly ISchedulesRepository _schedulesRepo;

        private readonly IMapper _mapper;
        private readonly ILogger<WorkGraphicService> _logger;
        private readonly IUserAccessInfoProvider _userAccessInfoProvider;

        private readonly IAccessFiltersStore _accessFiltersStore;

        private readonly IApplicationContextService _contextService;
        private readonly IAuthService _authService;

        private AccessFilter<ScheduleEntity> _scheduleAccessFilter =>
            _accessFiltersStore.GetFilter<ScheduleEntity>();

        private AccessFilter<DailyInfoEntity> _dailyInfoAccessFilter =>
            _accessFiltersStore.GetFilter<DailyInfoEntity>();

        public WorkGraphicService(
            IDailyInfosRepository dailyInfosRepo,
            ISchedulesRepository schedulesRepo,
            IMapper mapper,
            IUserAccessInfoProvider userAccessInfoProvider,
            IAccessFiltersStore accessFiltersStore,
            IApplicationContextService contextService,
            IOptions<WorkGraphicOptions> options,
            IAuthService authService,
            ILogger<WorkGraphicService> logger
        )
        {
            _dailyInfosRepo = dailyInfosRepo;
            _schedulesRepo = schedulesRepo;
            _mapper = mapper;
            _userAccessInfoProvider = userAccessInfoProvider;
            _accessFiltersStore = accessFiltersStore;
            _contextService = contextService;
            _options = options;
            _authService = authService;
            _logger = logger;
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

            var schedule = await _schedulesRepo.GetAsync(
                new ScheduleRequest(
                    new EntityIdFilter<ScheduleEntity>(scheduleId).And(_scheduleAccessFilter)
                )
                {
                    IncludeShifts = true,
                    IncludeTemplate = true
                }
            );

            if (schedule == null)
            {
                return [];
            }

            if (schedule.IsGraphicUpdateRequired)
            {
                var graphic = await RecalculateAsync(schedule, cancellation);

                schedule.IsGraphicUpdateRequired = false;
                _schedulesRepo.Update(schedule);
                await _schedulesRepo.SaveChangesAsync(cancellation);

                return graphic;
            }

            var filter = new Specification<DailyInfoEntity>(x =>
                x.ScheduleId == scheduleId && x.Date >= startDate && x.Date <= endDate
            ).And(_dailyInfoAccessFilter);

            var entities = await _dailyInfosRepo.GetManyAsync(
                new PaginatedRequest<DailyInfoEntity>(filter, request.PageIndex, request.Amount),
                cancellation
            );

            if (_authService.IsUserInRoles("Vip") && entities.Count < request.Amount)
            {
                entities = LiveCalculation(schedule, startDate, endDate);
            }

            return _mapper.Map<List<DailyInfoDto>>(entities).OrderBy(x => x.Date).ToList();
        }

        private async Task<List<DailyInfoDto>> RecalculateAsync(
            ScheduleEntity schedule,
            CancellationToken cancellation = default
        )
        {
            var timer = new Stopwatch();

            timer.Start();

            _logger.LogInformation("Расписание: {1}. Начат расчёт графика", schedule.Name);
            await _contextService.CreateTransactionAsync(cancellation);

            var year = DateTime.Now.Year;

            var startDate = new DateOnly(year, 1, 1);
            var endDate = new DateOnly(year, 12, 31);

            if (_authService.IsUserInRoles("Vip"))
            {
                startDate = startDate.AddYears(-_options.Value.Vip.YearsBackward);
                endDate = endDate.AddYears(_options.Value.Vip.YearsForward);
            }
            else
            {
                startDate = startDate.AddYears(-_options.Value.Default.YearsBackward);
                endDate = endDate.AddYears(_options.Value.Default.YearsForward);
            }

            var graphic = new List<DailyInfoEntity>();

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var info = GetDailyInfo(schedule, date);

                graphic.Add(info);
            }

            await _dailyInfosRepo.DeleteByConditionAsync(
                new Specification<DailyInfoEntity>(x => x.ScheduleId == schedule.Id),
                cancellation
            );

            var result = await _dailyInfosRepo.CreateManyAsync(graphic, cancellation);

            timer.Stop();
            if (result.IsSucceed)
            {
                await _dailyInfosRepo.SaveChangesAsync(cancellation);

                await _contextService.CommitTransactionAsync(cancellation);

                _logger.LogInformation(
                    "Расписание: {1}. Расчёт графика окончен ({2} ms)",
                    schedule.Name,
                    timer.ElapsedMilliseconds
                );
                _logger.LogInformation(
                    "Расписание: {1}. Рассчитано дней: {2}",
                    schedule.Name,
                    graphic.Count
                );

                return _mapper.Map<List<DailyInfoDto>>(graphic).OrderBy(x => x.Date).ToList();
            }
            else
            {
                await _contextService.RollbackTransactionAsync(cancellation);
                _logger.LogInformation(
                    "Расписание: {1}. Ошибка расчёта графика ({2} ms)",
                    schedule.Name,
                    timer.ElapsedMilliseconds
                );
                return [];
            }

            //var filter = new Specification<DailyInfoEntity>(x => x.ScheduleId == schedule.Id).And(
            //    _dailyInfoAccessFilter
            //);

            //var daysAmount = await _dailyInfosRepo.GetAmountAsync(
            //    new EntityRequest<DailyInfoEntity>(filter),
            //    cancellation
            //);

            //if (daysAmount == 0)
            //{
            //    return [];
            //}

            //var first = await _dailyInfosRepo.GetManyAsync(
            //    new PaginatedRequest<DailyInfoEntity>(filter, 1, 1),
            //    cancellation
            //);

            //var last = await _dailyInfosRepo.GetManyAsync(
            //    new PaginatedRequest<DailyInfoEntity>(filter, daysAmount, 1),
            //    cancellation
            //);

            //var startDate = first.Single().Date;
            //var endDate = last.Single().Date;

            //var graphic = new List<DailyInfoEntity>();

            //for (var date = startDate; date <= endDate; date = date.AddDays(1))
            //{
            //    var info = GetDailyInfo(schedule, date);

            //    graphic.Add(info);
            //}

            //await _contextService.CreateTransactionAsync(cancellation);

            //await _dailyInfosRepo.DeleteByConditionAsync(
            //    new Specification<DailyInfoEntity>(x => x.ScheduleId == schedule.Id),
            //    cancellation
            //);

            //var result = await _dailyInfosRepo.CreateManyAsync(graphic, cancellation);

            //if (result.IsSucceed)
            //{
            //    await _dailyInfosRepo.SaveChangesAsync(cancellation);

            //    await _contextService.CommitTransactionAsync(cancellation);

            //    return _mapper.Map<List<DailyInfoDto>>(graphic).OrderBy(x => x.Date).ToList();
            //}
            //else
            //{
            //    await _contextService.RollbackTransactionAsync(cancellation);
            //    return [];
            //}
        }

        private List<DailyInfoEntity> LiveCalculation(
            ScheduleEntity schedule,
            DateOnly start,
            DateOnly end
        )
        {
            var timer = new Stopwatch();
            timer.Start();
            var grahpic = new List<DailyInfoEntity>();

            for (DateOnly date = start; date <= end; date = date.AddDays(1))
            {
                var info = GetDailyInfo(schedule, date);

                grahpic.Add(info);
            }
            timer.Stop();
            _logger.LogInformation(
                "Расписание {1}: Расчёт расписания. Период: {2} - {3}. ({4} ms)",
                schedule.Name,
                start,
                end,
                timer.ElapsedMilliseconds
            );
            return grahpic.OrderBy(x => x.Date).ToList();
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
