using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts.Special;
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
using WorkyOne.Domain.Requests.Schedule.Common;
using WorkyOne.Domain.Specifications.AccesFilters.Schedule.Common;
using WorkyOne.Domain.Specifications.AccesFilters.Schedule.Shifts;
using WorkyOne.Domain.Specifications.Base;
using WorkyOne.Domain.Specifications.Common;

namespace WorkyOne.AppServices.Services.Schedule.Shifts.Special
{
    public sealed class PeriodicShiftsService : IPeriodicShiftsService
    {
        private readonly ISchedulesRepository _scheduleRepo;
        private readonly IPeriodicShiftsRepository _shiftsRepo;
        private readonly IMapper _mapper;
        private readonly IEntityUpdateUtility _entityUpdater;

        private ScheduleAccessFilter _scheduleAccessFilter;
        private PeriodicShiftAccessFilter _shiftAccessFilter;

        public PeriodicShiftsService(
            ISchedulesRepository scheduleRepo,
            IPeriodicShiftsRepository shiftsRepo,
            IMapper mapper,
            IUserAccessInfoProvider userAccessInfoProvider,
            IEntityUpdateUtility entityUpdater
        )
        {
            _scheduleRepo = scheduleRepo;
            _shiftsRepo = shiftsRepo;
            _mapper = mapper;

            InitAccessFilters(userAccessInfoProvider).Wait();
            _entityUpdater = entityUpdater;
        }

        public async Task<RepositoryResult> CreateAsync(
            ShiftReferenceModel<PeriodicShiftDto> model,
            CancellationToken cancellation = default
        )
        {
            var filter = new EntityIdFilter<ScheduleEntity>(model.ParentId).And(
                _scheduleAccessFilter
            );

            var schedule = await _scheduleRepo.GetAsync(new ScheduleRequest(filter), cancellation);

            if (cancellation.IsCancellationRequested)
            {
                return RepositoryResult.CancellationRequested();
            }

            if (schedule == null)
            {
                return RepositoryResult.Error(ResultType.NotFound, model.ParentId, "Schedule");
            }

            var shift = _mapper.Map<PeriodicShiftEntity>(model.Shift);
            shift.Schedule = schedule;

            var result = await _shiftsRepo.CreateAsync(shift, cancellation);

            if (result.IsSucceed)
            {
                await _shiftsRepo.SaveChangesAsync(cancellation);
            }

            return result;
        }

        public async Task<RepositoryResult> DeleteAsync(
            string id,
            CancellationToken cancellation = default
        )
        {
            var filter = new EntityIdFilter<PeriodicShiftEntity>(id).And(_shiftAccessFilter);

            var deleted = await _shiftsRepo.GetAsync(
                new EntityRequest<PeriodicShiftEntity>(filter),
                cancellation
            );

            if (deleted == null)
            {
                return RepositoryResult.Error(ResultType.NotFound, id, nameof(PeriodicShiftEntity));
            }

            var result = _shiftsRepo.Delete(deleted);

            if (result.IsSucceed)
            {
                await _shiftsRepo.SaveChangesAsync(cancellation);
            }

            return result;
        }

        public async Task<PeriodicShiftDto?> GetAsync(
            string id,
            CancellationToken cancellation = default
        )
        {
            var filter = new EntityIdFilter<PeriodicShiftEntity>(id).And(_shiftAccessFilter);

            var entity = await _shiftsRepo.GetAsync(
                new EntityRequest<PeriodicShiftEntity>(filter),
                cancellation
            );

            if (entity == null)
            {
                return null;
            }

            var dto = _mapper.Map<PeriodicShiftDto>(entity);
            return dto;
        }

        public async Task<List<PeriodicShiftDto>> GetByScheduleIdAsync(
            string scheduleId,
            PaginatedRequest request,
            CancellationToken cancellation = default
        )
        {
            var filter = new Specification<PeriodicShiftEntity>(x =>
                x.ScheduleId == scheduleId
            ).And(_shiftAccessFilter);

            var entities = await _shiftsRepo.GetManyAsync(
                new PaginatedRequest<PeriodicShiftEntity>(
                    filter,
                    request.PageIndex,
                    request.Amount
                ),
                cancellation
            );

            var dtos = _mapper.Map<List<PeriodicShiftDto>>(entities);
            return dtos;
        }

        public async Task<List<PeriodicShiftDto>> GetManyAsync(
            PaginatedRequest request,
            CancellationToken cancellation = default
        )
        {
            var entities = await _shiftsRepo.GetManyAsync(
                new PaginatedRequest<PeriodicShiftEntity>(
                    _shiftAccessFilter,
                    request.PageIndex,
                    request.Amount
                ),
                cancellation
            );

            var dtos = _mapper.Map<List<PeriodicShiftDto>>(entities);
            return dtos;
        }

        public async Task<RepositoryResult> UpdateAsync(
            PeriodicShiftDto dto,
            CancellationToken cancellation = default
        )
        {
            var filter = new EntityIdFilter<PeriodicShiftEntity>(dto.Id).And(_shiftAccessFilter);

            var target = await _shiftsRepo.GetAsync(
                new EntityRequest<PeriodicShiftEntity>(filter),
                cancellation
            );

            if (target == null)
            {
                return RepositoryResult.Error(
                    ResultType.NotFound,
                    dto.Id,
                    nameof(PeriodicShiftEntity)
                );
            }

            var source = _mapper.Map<PeriodicShiftEntity>(dto);

            _entityUpdater.Update(target, source);

            var result = _shiftsRepo.Update(target);

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

        private async Task InitAccessFilters(IUserAccessInfoProvider accessProvider)
        {
            var accessInfo = await accessProvider.GetCurrentAsync();

            _scheduleAccessFilter = new ScheduleAccessFilter(accessInfo);
            _shiftAccessFilter = new PeriodicShiftAccessFilter(accessInfo);
        }
    }
}
