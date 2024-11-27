using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services.Users;
using WorkyOne.AppServices.Interfaces.Stores;
using WorkyOne.AppServices.Interfaces.Utilities;
using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Contracts.Enums.Result;
using WorkyOne.Contracts.Repositories.Result;
using WorkyOne.Contracts.Services.CreateModels.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Shifts.Special;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Domain.Requests.Schedule.Common;
using WorkyOne.Domain.Specifications.AccesFilters.Abstractions;
using WorkyOne.Domain.Specifications.AccesFilters.Schedule.Common;
using WorkyOne.Domain.Specifications.AccesFilters.Schedule.Shifts;
using WorkyOne.Domain.Specifications.Base;
using WorkyOne.Domain.Specifications.Common;

namespace WorkyOne.AppServices.Services.Schedule.Common
{
    /// <summary>
    /// Сервис по работе с шаблонами
    /// </summary>
    public sealed class TemplateService : ITemplateService
    {
        private const string _positionsError = "Неверно указаны номера позиций в сменах";

        private readonly ITemplatesRepository _templatesRepo;
        private readonly ISchedulesRepository _schedulesRepo;
        private readonly ITemplatedShiftsRepository _templatedShiftsRepo;

        private readonly IMapper _mapper;
        private readonly IEntityUpdateUtility _updateUtility;
        private readonly IAccessFiltersStore _accessFiltersStore;

        private AccessFilter<TemplateEntity> _templateFilter =>
            _accessFiltersStore.GetFilter<TemplateEntity>();
        private AccessFilter<ScheduleEntity> _scheduleFilter =>
            _accessFiltersStore.GetFilter<ScheduleEntity>();
        private AccessFilter<TemplatedShiftEntity> _shiftFilter =>
            _accessFiltersStore.GetFilter<TemplatedShiftEntity>();

        public TemplateService(
            ITemplatesRepository templatesRepo,
            IMapper mapper,
            IEntityUpdateUtility updateUtility,
            ISchedulesRepository schedulesRepo,
            IUserAccessInfoProvider accessInfoProvider,
            ITemplatedShiftsRepository templatedShiftsRepo,
            IAccessFiltersStore accessFiltersStore
        )
        {
            _templatesRepo = templatesRepo;
            _mapper = mapper;
            _updateUtility = updateUtility;
            _schedulesRepo = schedulesRepo;
            _templatedShiftsRepo = templatedShiftsRepo;
            _accessFiltersStore = accessFiltersStore;
        }

        public async Task<RepositoryResult> CreateAsync(
            TemplateModel model,
            CancellationToken cancellation = default
        )
        {
            var filter = new EntityIdFilter<ScheduleEntity>(model.ScheduleId).And(_scheduleFilter);

            var request = new ScheduleRequest(filter) { IncludeTemplate = true };

            var schedule = await _schedulesRepo.GetAsync(request, cancellation);

            if (schedule == null)
            {
                return RepositoryResult.Error(
                    ResultType.NotFound,
                    model.ScheduleId,
                    nameof(ScheduleEntity)
                );
            }

            if (schedule.Template != null)
            {
                var repoResult = RepositoryResult.Error(
                    ResultType.AlreadyExisted,
                    schedule.Template.Id,
                    nameof(TemplateEntity)
                );

                repoResult.Message = "У указанного расписания уже есть шаблон";
                return repoResult;
            }

            var entity = _mapper.Map<TemplateEntity>(model.Template);

            entity.Id = Guid.NewGuid().ToString();
            entity.ScheduleId = schedule.Id;

            CorrectShiftsPositions(entity.Shifts);

            var result = await _templatesRepo.CreateAsync(entity, cancellation);

            if (cancellation.IsCancellationRequested)
            {
                return RepositoryResult.CancellationRequested();
            }

            if (result.IsSucceed)
            {
                await _templatesRepo.SaveChangesAsync(cancellation);
            }

            return result;
        }

        public async Task<RepositoryResult> DeleteAsync(
            string id,
            CancellationToken cancellation = default
        )
        {
            var filter = new EntityIdFilter<TemplateEntity>(id).And(_templateFilter);

            var request = new EntityRequest<TemplateEntity>(filter);
            var deleted = await _templatesRepo.GetAsync(request, cancellation);

            if (deleted == null)
            {
                return RepositoryResult.Error(ResultType.NotFound, id, nameof(TemplateEntity));
            }

            var result = _templatesRepo.Delete(deleted);

            if (result.IsSucceed)
            {
                await _templatesRepo.SaveChangesAsync(cancellation);
            }
            return result;
        }

        public async Task<TemplateDto?> GetByScheduleIdAsync(
            string scheduleId,
            CancellationToken cancellation = default
        )
        {
            var filter = new Specification<TemplateEntity>(x => x.ScheduleId == scheduleId).And(
                _templateFilter
            );

            var entity = await _templatesRepo.GetAsync(
                new EntityRequest<TemplateEntity>(filter),
                cancellation
            );

            if (entity == null)
            {
                return null;
            }

            var dto = _mapper.Map<TemplateDto>(entity);
            dto.Shifts = dto.Shifts.OrderBy(x => x.Position).ToList();
            return dto;
        }

        public async Task<RepositoryResult> UpdateAsync(
            TemplateEntity target,
            TemplateEntity source,
            CancellationToken cancellation = default
        )
        {
            CorrectShiftsPositions(source.Shifts);

            _updateUtility.Update(target, source);
            //UpdateTemplatedShifts(target, source.Shifts);

            source.Shifts.ForEach(x => x.TemplateId = source.Id);

            var result = _templatedShiftsRepo.Renew(target.Shifts, source.Shifts);

            if (!result.IsSucceed)
            {
                return result;
            }

            result = _templatesRepo.Update(target);

            if (result.IsSucceed)
            {
                await _templatesRepo.SaveChangesAsync(cancellation);

                if (cancellation.IsCancellationRequested)
                {
                    return RepositoryResult.CancellationRequested();
                }
            }

            return result;
        }

        private Task<List<TemplatedShiftEntity>> GetTemplatedShiftAsync(
            string templateId,
            CancellationToken cancellation = default
        )
        {
            var filter = new Specification<TemplatedShiftEntity>(x =>
                x.TemplateId == templateId
            ).And(_shiftFilter);

            return _templatedShiftsRepo.GetManyAsync(
                new PaginatedRequest<TemplatedShiftEntity>(filter, 1, 100)
            );
        }

        private void CorrectShiftsPositions(IEnumerable<TemplatedShiftEntity> shifts)
        {
            var ordered = shifts.OrderBy(x => x.Position).ToList();

            var i = 1;

            foreach (var shift in ordered)
            {
                shift.Position = i++;
            }
        }

        private void UpdateTemplatedShifts(
            TemplateEntity template,
            IEnumerable<TemplatedShiftEntity> shifts
        )
        {
            foreach (var shift in shifts)
            {
                shift.TemplateId = template.Id;
                shift.Template = template;
            }

            template.Shifts = shifts.ToList();
        }
    }
}
