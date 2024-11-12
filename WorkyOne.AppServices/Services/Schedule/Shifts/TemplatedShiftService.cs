using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts;
using WorkyOne.AppServices.Interfaces.Services.Users;
using WorkyOne.AppServices.Interfaces.Utilities;
using WorkyOne.Contracts.DTOs.Schedule.Shifts.Special;
using WorkyOne.Contracts.Enums.Result;
using WorkyOne.Contracts.Repositories.Result;
using WorkyOne.Contracts.Services.CreateModels.Schedule.Shifts;
using WorkyOne.Contracts.Services.GetRequests.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Shifts.Special;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Domain.Specifications.AccesFilters.Schedule.Common;
using WorkyOne.Domain.Specifications.AccesFilters.Schedule.Shifts;
using WorkyOne.Domain.Specifications.Base;
using WorkyOne.Domain.Specifications.Common;

namespace WorkyOne.AppServices.Services.Schedule.Shifts
{
    /// <summary>
    /// Сервис по работе с "шаблонными" сменамиы
    /// </summary>
    public class TemplatedShiftService : ITemplatedShiftService
    {
        private readonly ITemplatedShiftsRepository _shiftsRepo;
        private readonly IMapper _mapper;
        private readonly ITemplatesRepository _templatesRepo;
        private readonly IEntityUpdateUtility _updateUtility;
        private readonly IUserAccessInfoProvider _userAccessInfoProvider;

        private TemplateAccessFilter _templateFilter;
        private TemplatedShiftAccessFilter _shiftFilter;

        public TemplatedShiftService(
            ITemplatedShiftsRepository shiftsRepo,
            IMapper mapper,
            ITemplatesRepository templatesRepo,
            IEntityUpdateUtility updateUtility,
            IUserAccessInfoProvider userAccessInfoProvider
        )
        {
            _shiftsRepo = shiftsRepo;
            _mapper = mapper;
            _templatesRepo = templatesRepo;
            _updateUtility = updateUtility;
            _userAccessInfoProvider = userAccessInfoProvider;

            InitFiltersAsync().Wait();
        }

        public async Task<RepositoryResult> CreateAsync(
            ShiftModel<TemplatedShiftDto> model,
            CancellationToken cancellation = default
        )
        {
            var filter = new EntityIdFilter<TemplateEntity>(model.ParentId).And(_templateFilter);
            var template = await _templatesRepo.GetAsync(
                new EntityRequest<TemplateEntity>(filter),
                cancellation
            );

            if (template == null)
            {
                return RepositoryResult.Error(
                    ResultType.NotFound,
                    model.ParentId,
                    nameof(TemplateEntity)
                );
            }

            var entity = _mapper.Map<TemplatedShiftEntity>(model.Shift);
            entity.Id = Guid.NewGuid().ToString();
            entity.TemplateId = template.Id;

            var result = await _shiftsRepo.CreateAsync(entity, cancellation);

            if (cancellation.IsCancellationRequested)
            {
                return RepositoryResult.CancellationRequested();
            }

            if (result.IsSucceed)
            {
                await _shiftsRepo.SaveChangesAsync(cancellation);

                if (cancellation.IsCancellationRequested)
                {
                    return RepositoryResult.CancellationRequested();
                }
            }

            return result;
        }

        public async Task<RepositoryResult> DeleteAsync(
            string id,
            CancellationToken cancellation = default
        )
        {
            var filter = new EntityIdFilter<TemplatedShiftEntity>(id).And(_shiftFilter);
            var deleted = await _shiftsRepo.GetAsync(
                new EntityRequest<TemplatedShiftEntity>(filter),
                cancellation
            );

            if (deleted == null)
            {
                return RepositoryResult.Error(
                    ResultType.NotFound,
                    id,
                    nameof(TemplatedShiftEntity)
                );
            }

            var result = _shiftsRepo.Delete(deleted);

            if (cancellation.IsCancellationRequested)
            {
                return RepositoryResult.CancellationRequested();
            }
            if (result.IsSucceed)
            {
                await _shiftsRepo.SaveChangesAsync(cancellation);

                if (cancellation.IsCancellationRequested)
                {
                    return RepositoryResult.CancellationRequested();
                }
            }

            return result;
        }

        public async Task<TemplatedShiftDto?> GetAsync(
            string id,
            CancellationToken cancellation = default
        )
        {
            var filter = new EntityIdFilter<TemplatedShiftEntity>(id).And(_shiftFilter);

            var entity = await _shiftsRepo.GetAsync(
                new EntityRequest<TemplatedShiftEntity>(filter),
                cancellation
            );

            if (entity == null)
            {
                return null;
            }

            var dto = _mapper.Map<TemplatedShiftDto>(entity);
            return dto;
        }

        public async Task<List<TemplatedShiftDto>> GetByScheduleIdAsync(
            string scheduleId,
            PaginatedRequest request,
            CancellationToken cancellation = default
        )
        {
            var filter = new Specification<TemplatedShiftEntity>(x =>
                x.Template.ScheduleId == scheduleId
            ).And(_shiftFilter);

            var entities = await _shiftsRepo.GetManyAsync(
                new PaginatedRequest<TemplatedShiftEntity>(
                    filter,
                    request.PageIndex,
                    request.Amount
                ),
                cancellation
            );

            var dtos = _mapper.Map<List<TemplatedShiftDto>>(entities);
            return dtos;
        }

        public async Task<List<TemplatedShiftDto>> GetByTemplateIdAsync(
            string templateId,
            PaginatedRequest request,
            CancellationToken cancellation = default
        )
        {
            var filter = new Specification<TemplatedShiftEntity>(x =>
                x.TemplateId == templateId
            ).And(_shiftFilter);

            var repoRequest = new PaginatedRequest<TemplatedShiftEntity>(
                filter,
                request.PageIndex,
                request.Amount
            );

            var entities = await _shiftsRepo.GetManyAsync(repoRequest, cancellation);

            var dtos = _mapper.Map<List<TemplatedShiftDto>>(entities);
            return dtos;
        }

        public async Task<List<TemplatedShiftDto>> GetManyAsync(
            PaginatedRequest request,
            CancellationToken cancellation = default
        )
        {
            var repoRequest = new PaginatedRequest<TemplatedShiftEntity>(
                _shiftFilter,
                request.PageIndex,
                request.Amount
            );

            var entities = await _shiftsRepo.GetManyAsync(repoRequest, cancellation);

            var dtos = _mapper.Map<List<TemplatedShiftDto>>(entities);
            return dtos;
        }

        public async Task<RepositoryResult> UpdateAsync(
            TemplatedShiftDto dto,
            CancellationToken cancellation = default
        )
        {
            var filter = new EntityIdFilter<TemplatedShiftEntity>(dto.Id).And(_shiftFilter);

            var target = await _shiftsRepo.GetAsync(
                new EntityRequest<TemplatedShiftEntity>(filter),
                cancellation
            );

            if (target == null)
            {
                return RepositoryResult.Error(
                    ResultType.NotFound,
                    dto.Id,
                    nameof(TemplatedShiftEntity)
                );
            }

            var source = _mapper.Map<TemplatedShiftEntity>(dto);

            _updateUtility.Update(target, source);

            var result = _shiftsRepo.Update(target);

            if (cancellation.IsCancellationRequested)
            {
                return RepositoryResult.CancellationRequested();
            }

            if (result.IsSucceed)
            {
                await _shiftsRepo.SaveChangesAsync(cancellation);

                if (cancellation.IsCancellationRequested)
                {
                    return RepositoryResult.CancellationRequested();
                }
            }

            return result;
        }

        private async Task InitFiltersAsync()
        {
            if (_templateFilter == null)
            {
                var accessInfo = await _userAccessInfoProvider.GetCurrentAsync();

                _templateFilter = new TemplateAccessFilter(accessInfo);
                _shiftFilter = new TemplatedShiftAccessFilter(accessInfo);
            }
        }
    }
}
