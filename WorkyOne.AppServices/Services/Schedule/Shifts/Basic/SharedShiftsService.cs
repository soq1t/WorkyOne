using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts.Basic;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts.Basic;
using WorkyOne.AppServices.Interfaces.Utilities;
using WorkyOne.Contracts.DTOs.Schedule.Shifts.Basic;
using WorkyOne.Contracts.Enums.Result;
using WorkyOne.Contracts.Repositories.Result;
using WorkyOne.Contracts.Services.GetRequests.Common;
using WorkyOne.Domain.Entities.Schedule.Shifts.Basic;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Domain.Specifications.Base;
using WorkyOne.Domain.Specifications.Common;

namespace WorkyOne.AppServices.Services.Schedule.Shifts.Basic
{
    /// <summary>
    /// Интерфейс сервиса по работе с "общими" сменами
    /// </summary>
    public class SharedShiftsService : ISharedShiftsService
    {
        private readonly ISharedShiftsRepository _shiftsRepository;

        private readonly IMapper _mapper;
        private readonly IEntityUpdateUtility _entityUpdater;

        public SharedShiftsService(
            ISharedShiftsRepository shiftsRepository,
            IMapper mapper,
            IEntityUpdateUtility entityUpdater
        )
        {
            _shiftsRepository = shiftsRepository;
            _mapper = mapper;
            _entityUpdater = entityUpdater;
        }

        public async Task<RepositoryResult> CreateAsync(
            SharedShiftDto dto,
            CancellationToken cancellation = default
        )
        {
            var entity = _mapper.Map<SharedShiftEntity>(dto);

            var result = await _shiftsRepository.CreateAsync(entity, cancellation);

            if (result.IsSucceed)
            {
                await _shiftsRepository.SaveChangesAsync(cancellation);
            }

            return result;
        }

        public async Task<RepositoryResult> DeleteAsync(
            string id,
            CancellationToken cancellation = default
        )
        {
            var entity = await _shiftsRepository.GetAsync(
                new EntityRequest<SharedShiftEntity>(new EntityIdFilter<SharedShiftEntity>(id)),
                cancellation
            );

            if (entity == null)
            {
                return RepositoryResult.Error(ResultType.NotFound, id, nameof(SharedShiftEntity));
            }

            var result = _shiftsRepository.Delete(entity);

            if (result.IsSucceed)
            {
                await _shiftsRepository.SaveChangesAsync(cancellation);
            }

            return result;
        }

        public async Task<SharedShiftDto?> GetAsync(
            string id,
            CancellationToken cancellation = default
        )
        {
            var entity = await _shiftsRepository.GetAsync(
                new EntityRequest<SharedShiftEntity>(new EntityIdFilter<SharedShiftEntity>(id)),
                cancellation
            );

            return _mapper.Map<SharedShiftDto?>(entity);
        }

        public async Task<List<SharedShiftDto>> GetManyAsync(
            PaginatedRequest request,
            CancellationToken cancellation = default
        )
        {
            var entities = await _shiftsRepository.GetManyAsync(
                new PaginatedRequest<SharedShiftEntity>(
                    new Specification<SharedShiftEntity>(x => true),
                    request.PageIndex,
                    request.Amount
                ),
                cancellation
            );

            return _mapper.Map<List<SharedShiftDto>>(entities);
        }

        public async Task<RepositoryResult> UpdateAsync(
            SharedShiftDto dto,
            CancellationToken cancellation = default
        )
        {
            var target = await _shiftsRepository.GetAsync(
                new EntityRequest<SharedShiftEntity>(new EntityIdFilter<SharedShiftEntity>(dto.Id)),
                cancellation
            );

            if (target == null)
            {
                return RepositoryResult.Error(
                    ResultType.NotFound,
                    dto.Id,
                    nameof(SharedShiftEntity)
                );
            }

            var source = _mapper.Map<SharedShiftEntity>(dto);

            _entityUpdater.Update(target, source);

            var result = _shiftsRepository.Update(target);

            if (result.IsSucceed)
            {
                await _shiftsRepository.SaveChangesAsync(cancellation);
            }

            return result;
        }
    }
}
