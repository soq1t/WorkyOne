using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services.Users;
using WorkyOne.AppServices.Interfaces.Utilities;
using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Contracts.Enums.Result;
using WorkyOne.Contracts.Repositories.Result;
using WorkyOne.Contracts.Services.CreateModels.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Shifts.Special;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Domain.Requests.Schedule.Common;
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

        private TemplateAccessFilter _templateFilter;
        private ScheduleAccessFilter _scheduleFilter;
        private TemplatedShiftAccessFilter _shiftFilter;

        public TemplateService(
            ITemplatesRepository templatesRepo,
            IMapper mapper,
            IEntityUpdateUtility updateUtility,
            ISchedulesRepository schedulesRepo,
            IUserAccessInfoProvider accessInfoProvider,
            ITemplatedShiftsRepository templatedShiftsRepo
        )
        {
            _templatesRepo = templatesRepo;
            _mapper = mapper;
            _updateUtility = updateUtility;
            _schedulesRepo = schedulesRepo;
            _templatedShiftsRepo = templatedShiftsRepo;

            InitFiltersAsync(accessInfoProvider).Wait();
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

            if (!CheckShiftsPositions(entity.Shifts))
            {
                return RepositoryResult.Error(_positionsError);
            }

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

        public async Task<TemplateDto?> GetAsync(
            string id,
            CancellationToken cancellation = default
        )
        {
            var filter = new EntityIdFilter<TemplateEntity>(id).And(_templateFilter);

            var entity = await _templatesRepo.GetAsync(
                new EntityRequest<TemplateEntity>(filter),
                cancellation
            );
            if (entity == null)
            {
                return null;
            }

            var dto = _mapper.Map<TemplateDto>(entity);
            return dto;
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
            return dto;
        }

        public async Task<RepositoryResult> UpdateAsync(
            TemplateDto dto,
            CancellationToken cancellation = default
        )
        {
            var filter = new EntityIdFilter<TemplateEntity>(dto.Id).And(_templateFilter);

            var target = await _templatesRepo.GetAsync(
                new EntityRequest<TemplateEntity>(filter),
                cancellation
            );

            if (target == null)
            {
                return RepositoryResult.Error(
                    ResultType.NotFound,
                    dto.Id,
                    nameof(RepositoryResult)
                );
            }

            var source = _mapper.Map<TemplateEntity>(dto);

            if (!CheckShiftsPositions(source.Shifts))
            {
                return RepositoryResult.Error(_positionsError);
            }

            _updateUtility.Update(target, source);

            await _templatedShiftsRepo.DeleteByConditionAsync(
                new Specification<TemplatedShiftEntity>(x => x.TemplateId == target.Id).And(
                    _shiftFilter
                ),
                cancellation
            );

            await _templatedShiftsRepo.CreateManyAsync(target.Shifts, cancellation);

            var result = _templatesRepo.Update(target);

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

        private async Task InitFiltersAsync(IUserAccessInfoProvider provider)
        {
            var accessInfo = await provider.GetCurrentAsync();

            _templateFilter = new TemplateAccessFilter(accessInfo);
            _scheduleFilter = new ScheduleAccessFilter(accessInfo);
            _shiftFilter = new TemplatedShiftAccessFilter(accessInfo);
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

        private bool CheckShiftsPositions(IEnumerable<TemplatedShiftEntity> shifts)
        {
            var amount = shifts.Count();

            for (var i = 1; i <= amount; i++)
            {
                if (shifts.Count(x => x.Position == i) != 1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
