using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Context;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts.Special;
using WorkyOne.AppServices.Interfaces.Stores;
using WorkyOne.AppServices.Interfaces.Utilities;
using WorkyOne.Contracts.DTOs.Schedule.Shifts.Special;
using WorkyOne.Contracts.Enums.Result;
using WorkyOne.Contracts.Repositories.Result;
using WorkyOne.Contracts.Services.GetRequests.Common;
using WorkyOne.Domain.Entities.Abstractions.Shifts;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Shifts.Special;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Domain.Requests.Schedule.Common;
using WorkyOne.Domain.Specifications.Base;
using WorkyOne.Domain.Specifications.Common;

namespace WorkyOne.AppServices.Services.Schedule.Shifts.Special
{
    /// <summary>
    /// Сервис по работе с <see cref="DatedShiftEntity"/>
    /// </summary>
    public sealed class DatedShiftsService : IDatedShiftsService
    {
        private readonly IDatedShiftsRepository _datedShiftsRepository;
        private readonly ISchedulesRepository _schedulesRepository;

        private readonly IApplicationContextService _contextService;

        private readonly IMapper _mapper;
        private readonly IEntityUpdateUtility _entityUpdateUtility;
        private readonly IReferenceShiftUtility _referenceShiftsUtility;

        private readonly IAccessFiltersStore _accessFilters;

        public DatedShiftsService(
            IDatedShiftsRepository datedShiftsRepository,
            IMapper mapper,
            ISchedulesRepository schedulesRepository,
            IEntityUpdateUtility entityUpdateUtility,
            IAccessFiltersStore accessFilters,
            IReferenceShiftUtility referenceShiftsUtility,
            IApplicationContextService contextService
        )
        {
            _datedShiftsRepository = datedShiftsRepository;
            _mapper = mapper;
            _schedulesRepository = schedulesRepository;
            _entityUpdateUtility = entityUpdateUtility;
            _accessFilters = accessFilters;
            _referenceShiftsUtility = referenceShiftsUtility;
            _contextService = contextService;
        }

        public async Task<DatedShiftDto?> GetAsync(
            string id,
            CancellationToken cancellation = default
        )
        {
            var filter = new EntityIdFilter<DatedShiftEntity>(id).And(
                _accessFilters.GetFilter<DatedShiftEntity>()
            );

            var entity = await _datedShiftsRepository.GetAsync(
                new EntityRequest<DatedShiftEntity>(filter),
                cancellation
            );

            if (entity == null)
            {
                return null;
            }

            var dto = _mapper.Map<DatedShiftDto>(entity);
            return dto;
        }

        public async Task<List<DatedShiftDto>> GetManyAsync(
            PaginatedRequest request,
            CancellationToken cancellation = default
        )
        {
            var pageIndex = request.PageIndex;
            var amount = request.Amount;

            var entities = await _datedShiftsRepository.GetManyAsync(
                new PaginatedRequest<DatedShiftEntity>(
                    _accessFilters.GetFilter<DatedShiftEntity>(),
                    pageIndex,
                    amount
                ),
                cancellation
            );

            var dtos = _mapper.Map<List<DatedShiftDto>>(entities);
            return dtos;
        }

        public async Task<List<DatedShiftDto>> GetForScheduleAsync(
            string scheduleId,
            PaginatedRequest request,
            CancellationToken cancellation = default
        )
        {
            var filter = new Specification<DatedShiftEntity>(x => x.ScheduleId == scheduleId).And(
                _accessFilters.GetFilter<DatedShiftEntity>()
            );

            var entities = await _datedShiftsRepository.GetManyAsync(
                new PaginatedRequest<DatedShiftEntity>(filter, request.PageIndex, request.Amount),
                cancellation
            );
            var dtos = _mapper.Map<List<DatedShiftDto>>(entities);

            return dtos;
        }

        public async Task<RepositoryResult> CreateAsync(
            DatedShiftDto dto,
            string scheduleId,
            CancellationToken cancellation = default
        )
        {
            await _contextService.CreateTransactionAsync(cancellation);

            var schedule = await _schedulesRepository.GetAsync(
                new ScheduleRequest(
                    new EntityIdFilter<ScheduleEntity>(scheduleId).And(
                        _accessFilters.GetFilter<ScheduleEntity>()
                    )
                ),
                cancellation
            );

            if (cancellation.IsCancellationRequested)
            {
                return RepositoryResult.CancellationRequested();
            }

            if (schedule == null)
            {
                return RepositoryResult.Error(
                    ResultType.NotFound,
                    scheduleId,
                    nameof(ScheduleEntity)
                );
            }

            var shift = await _referenceShiftsUtility.GetReferenceShift(dto.ShiftId, cancellation);

            if (shift == null)
            {
                return RepositoryResult.Error(
                    ResultType.NotFound,
                    dto.ShiftId,
                    nameof(ShiftEntity)
                );
            }

            var entity = _mapper.Map<DatedShiftEntity>(dto);

            entity.Schedule = schedule;
            entity.Id = Guid.NewGuid().ToString();
            entity.Shift = shift;
            entity.ShiftId = shift.Id;

            var result = await _datedShiftsRepository.CreateAsync(entity, cancellation);
            if (cancellation.IsCancellationRequested)
            {
                return RepositoryResult.CancellationRequested();
            }

            if (result.IsSucceed)
            {
                schedule.IsGraphicUpdateRequired = true;
                _schedulesRepository.Update(schedule);

                await _contextService.SaveChangesAsync(cancellation);
                await _contextService.CommitTransactionAsync(cancellation);
            }
            else
            {
                await _contextService.RollbackTransactionAsync(cancellation);
            }

            return result;
        }

        public async Task<RepositoryResult> UpdateAsync(
            DatedShiftDto dto,
            CancellationToken cancellation = default
        )
        {
            await _contextService.CreateTransactionAsync(cancellation);

            var filter = new EntityIdFilter<DatedShiftEntity>(dto.Id).And(
                _accessFilters.GetFilter<DatedShiftEntity>()
            );
            var target = await _datedShiftsRepository.GetAsync(
                new EntityRequest<DatedShiftEntity>(filter),
                cancellation
            );

            if (target == null)
            {
                return RepositoryResult.Error(
                    ResultType.NotFound,
                    dto.Id,
                    nameof(DatedShiftEntity)
                );
            }

            var source = _mapper.Map<DatedShiftEntity>(dto);

            _entityUpdateUtility.Update(target, source);

            var result = _datedShiftsRepository.Update(target);

            if (result.IsSucceed)
            {
                var schedule = await _schedulesRepository.GetAsync(
                    new ScheduleRequest(new EntityIdFilter<ScheduleEntity>(target.ScheduleId)),
                    cancellation
                );

                if (schedule != null)
                {
                    schedule.IsGraphicUpdateRequired = true;
                    _schedulesRepository.Update(schedule);
                }

                await _contextService.SaveChangesAsync(cancellation);
                await _contextService.CommitTransactionAsync(cancellation);
            }
            else
            {
                await _contextService.RollbackTransactionAsync(cancellation);
            }

            return result;
        }

        public async Task<RepositoryResult> DeleteAsync(
            string id,
            CancellationToken cancellation = default
        )
        {
            await _contextService.CreateTransactionAsync(cancellation);

            var filter = new EntityIdFilter<DatedShiftEntity>(id).And(
                _accessFilters.GetFilter<DatedShiftEntity>()
            );
            var deleted = await _datedShiftsRepository.GetAsync(
                new EntityRequest<DatedShiftEntity>(filter),
                cancellation
            );

            if (deleted == null)
            {
                return RepositoryResult.Error(ResultType.NotFound, id, nameof(DatedShiftEntity));
            }

            var result = _datedShiftsRepository.Delete(deleted);

            if (result.IsSucceed)
            {
                var schedule = await _schedulesRepository.GetAsync(
                    new ScheduleRequest(new EntityIdFilter<ScheduleEntity>(deleted.ScheduleId)),
                    cancellation
                );

                if (schedule != null)
                {
                    schedule.IsGraphicUpdateRequired = true;
                    _schedulesRepository.Update(schedule);
                }

                await _contextService.SaveChangesAsync(cancellation);
                await _contextService.CommitTransactionAsync(cancellation);
            }
            else
            {
                await _contextService.RollbackTransactionAsync(cancellation);
            }

            return result;
        }
    }
}
