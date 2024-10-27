using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Users;
using WorkyOne.AppServices.Interfaces.Utilities;
using WorkyOne.Contracts.DTOs.Schedule.Shifts;
using WorkyOne.Contracts.Services.Common;
using WorkyOne.Contracts.Services.GetRequests.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Shifts;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Domain.Requests.Schedule.Common;
using WorkyOne.Domain.Specifications.AccesFilters.Schedule.Common;
using WorkyOne.Domain.Specifications.AccesFilters.Schedule.Shifts;
using WorkyOne.Domain.Specifications.Base;
using WorkyOne.Domain.Specifications.Common;

namespace WorkyOne.AppServices.Services.Schedule.Shifts
{
    /// <summary>
    /// Сервис по работе с <see cref="DatedShiftEntity"/>
    /// </summary>
    public sealed class DatedShiftsService : IDatedShiftsService
    {
        private readonly IDatedShiftsRepository _datedShiftsRepository;
        private readonly ISchedulesRepository _schedulesRepository;
        private readonly IMapper _mapper;

        private readonly IEntityUpdateUtility _entityUpdateUtility;
        private readonly IUserAccessInfoProvider _userAccessInfoProvider;

        private DatedShiftAccessFilter _shiftAccessFilter;
        private ScheduleAccessFilter _scheduleAccessFilter;

        public DatedShiftsService(
            IDatedShiftsRepository datedShiftsRepository,
            IMapper mapper,
            ISchedulesRepository schedulesRepository,
            IEntityUpdateUtility entityUpdateUtility,
            IUserAccessInfoProvider userAccessInfoProvider
        )
        {
            _datedShiftsRepository = datedShiftsRepository;
            _mapper = mapper;
            _schedulesRepository = schedulesRepository;
            _entityUpdateUtility = entityUpdateUtility;
            _userAccessInfoProvider = userAccessInfoProvider;

            InitAccessFiltersAsync().Wait();
        }

        public async Task<DatedShiftDto?> GetAsync(
            string id,
            CancellationToken cancellation = default
        )
        {
            var filter = new EntityIdFilter<DatedShiftEntity>(id).And(_shiftAccessFilter);

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
                new PaginatedRequest<DatedShiftEntity>(_shiftAccessFilter, pageIndex, amount),
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
                _shiftAccessFilter
            );

            var entities = await _datedShiftsRepository.GetManyAsync(
                new PaginatedRequest<DatedShiftEntity>(filter, request.PageIndex, request.Amount),
                cancellation
            );
            var dtos = _mapper.Map<List<DatedShiftDto>>(entities);

            return dtos;
        }

        public async Task<ServiceResult> CreateAsync(
            DatedShiftDto dto,
            string scheduleId,
            CancellationToken cancellation = default
        )
        {
            var filter = new EntityIdFilter<ScheduleEntity>(scheduleId).And(_scheduleAccessFilter);
            var schedule = await _schedulesRepository.GetAsync(
                new ScheduleRequest(filter),
                cancellation
            );

            if (cancellation.IsCancellationRequested)
            {
                return ServiceResult.CancellationRequested();
            }

            if (schedule == null)
            {
                return ServiceResult.Error($"Указанное расписание (ID: {scheduleId}) не найдено");
            }

            DatedShiftEntity shift = _mapper.Map<DatedShiftEntity>(dto);
            shift.Schedule = schedule;
            shift.Id = Guid.NewGuid().ToString();

            var result = await _datedShiftsRepository.CreateAsync(shift, cancellation);
            if (cancellation.IsCancellationRequested)
            {
                return ServiceResult.CancellationRequested();
            }

            if (result.IsSuccess)
            {
                await _datedShiftsRepository.SaveChangesAsync(cancellation);
                return ServiceResult.Ok($"Датированная смена (ID: {shift.Id}) успешно создана!");
            }
            else
            {
                return ServiceResult.FromRepositoryResult(result);
            }
        }

        public async Task<ServiceResult> UpdateAsync(
            DatedShiftDto dto,
            CancellationToken cancellation = default
        )
        {
            var filter = new EntityIdFilter<DatedShiftEntity>(dto.Id).And(_shiftAccessFilter);
            var target = await _datedShiftsRepository.GetAsync(
                new EntityRequest<DatedShiftEntity>(filter),
                cancellation
            );

            if (target == null)
            {
                return ServiceResult.Error($"Датированная смена (ID: {dto.Id}) не была найдена");
            }

            var source = _mapper.Map<DatedShiftEntity>(dto);

            _entityUpdateUtility.Update(target, source);

            var result = _datedShiftsRepository.Update(target);

            if (result.IsSuccess)
            {
                await _datedShiftsRepository.SaveChangesAsync(cancellation);
                return ServiceResult.Ok($"Датированная смена (ID: {target.Id}) успешно обновлена");
            }
            else
            {
                return ServiceResult.FromRepositoryResult(result);
            }
        }

        public async Task<ServiceResult> DeleteAsync(
            string id,
            CancellationToken cancellation = default
        )
        {
            var filter = new EntityIdFilter<DatedShiftEntity>(id).And(_shiftAccessFilter);
            var deleted = await _datedShiftsRepository.GetAsync(
                new EntityRequest<DatedShiftEntity>(filter),
                cancellation
            );

            if (deleted == null)
            {
                return ServiceResult.Error($"Датированная смена (ID: {id}) не найдена");
            }

            var result = _datedShiftsRepository.Delete(deleted);

            if (result.IsSuccess)
            {
                await _datedShiftsRepository.SaveChangesAsync(cancellation);
                return ServiceResult.Ok($"Датированная смена (ID: {id}) успешно удалена");
            }
            else
            {
                return ServiceResult.FromRepositoryResult(result);
            }
        }

        private async Task InitAccessFiltersAsync()
        {
            if (_scheduleAccessFilter == null || _shiftAccessFilter == null)
            {
                var accessInfo = await _userAccessInfoProvider.GetCurrentAsync();

                _scheduleAccessFilter = new ScheduleAccessFilter(accessInfo);
                _shiftAccessFilter = new DatedShiftAccessFilter(accessInfo);
            }
        }
    }
}
