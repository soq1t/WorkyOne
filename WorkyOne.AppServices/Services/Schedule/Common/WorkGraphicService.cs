using System.Text;
using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Contracts.Repositories.Requests.Common;
using WorkyOne.Contracts.Services;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Requests.Schedule.Common;

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

        public WorkGraphicService(
            IDailyInfosRepository dailyInfosRepo,
            ISchedulesRepository schedulesRepo,
            IMapper mapper
        )
        {
            _dailyInfosRepo = dailyInfosRepo;
            _schedulesRepo = schedulesRepo;
            _mapper = mapper;
        }

        public async Task<ServiceResult> ClearAsync(
            string scheduleId,
            CancellationToken cancellation = default
        )
        {
            var result = await _dailyInfosRepo.DeleteByConditionAsync(
                (x) => x.ScheduleId == scheduleId,
                cancellation
            );

            if (result.IsSuccess)
            {
                return ServiceResult.Ok(
                    $"График для расписания (ID: {scheduleId}) был полностью очищен!"
                );
            }
            else
            {
                return ServiceResult.FromRepositoryResult(result);
            }
        }

        public async Task<ServiceResult> ClearRangeAsync(
            WorkGraphicRequest request,
            CancellationToken cancellation = default
        )
        {
            var startDate = request.StartDate.Value;
            var endDate = request.EndDate.Value;
            var scheduleId = request.ScheduleId;

            var result = await _dailyInfosRepo.DeleteByConditionAsync(
                (x) => x.ScheduleId == scheduleId && x.Date >= startDate && x.Date <= endDate,
                cancellation
            );

            if (result.IsSuccess)
            {
                var message = new StringBuilder(
                    $"Рабочий график для расписания (ID: {scheduleId}) был успешно очищен!"
                );
                message.AppendLine($"Период очистки: {startDate} - {endDate}");
                return ServiceResult.Ok(message.ToString());
            }
            else
            {
                return ServiceResult.FromRepositoryResult(result);
            }
        }

        public async Task<ServiceResult> CreateAsync(
            WorkGraphicRequest request,
            CancellationToken cancellation = default
        )
        {
            var schedule = await _schedulesRepo.GetAsync(
                new ScheduleRequest
                {
                    EntityId = request.ScheduleId,
                    IncludeTemplate = true,
                    IncludeShifts = true,
                },
                cancellation
            );

            if (schedule == null)
            {
                return ServiceResult.Error($"Не найдено расписание с ID: {request.ScheduleId}");
            }

            var endDate = request.EndDate.Value;
            var startDate = request.StartDate.Value;

            var infos = new List<DailyInfoEntity>(endDate.DayNumber - startDate.DayNumber + 1);

            for (DateOnly date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (cancellation.IsCancellationRequested)
                {
                    return ServiceResult.CancellationRequested();
                }
                DailyInfoEntity info = GetDailyInfo(schedule, date);
                infos.Add(info);
            }

            await ClearRangeAsync(request, cancellation);

            var result = await _dailyInfosRepo.CreateManyAsync(infos, cancellation);

            if (result.IsSuccess)
            {
                await _dailyInfosRepo.SaveChangesAsync(cancellation);

                var message = new StringBuilder(
                    $"Рабочий график для расписания (ID: {request.ScheduleId}) был успешно создан!\n"
                );
                message.AppendLine(
                    $"Промежуток созданного графика: {request.StartDate} - {request.EndDate}"
                );
                message.AppendLine($"Затронуто дней: {infos.Count}");

                return ServiceResult.Ok(message.ToString());
            }
            else
            {
                return ServiceResult.FromRepositoryResult(result);
            }
        }

        public async Task<List<DailyInfoDto>> GetGraphicAsync(
            WorkGraphicRequest request,
            CancellationToken cancellation = default
        )
        {
            var scheduleId = request.ScheduleId;
            var startDate = request.StartDate.Value;
            var endDate = request.EndDate.Value;

            var repoRequest = new PaginatedDailyInfoRequest(startDate, endDate, scheduleId);

            var entities = await _dailyInfosRepo.GetManyAsync(repoRequest);

            var dtos = _mapper.Map<List<DailyInfoDto>>(entities);

            return dtos;
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
