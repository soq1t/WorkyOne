using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts;
using WorkyOne.AppServices.Interfaces.Utilities;
using WorkyOne.Contracts.DTOs.Schedule.Shifts;
using WorkyOne.Contracts.Services.Common;
using WorkyOne.Contracts.Services.GetRequests.Common;
using WorkyOne.Domain.Entities.Schedule.Shifts;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Domain.Requests.Schedule.Common;
using WorkyOne.Domain.Requests.Schedule.Shifts;

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

        public DatedShiftsService(
            IDatedShiftsRepository datedShiftsRepository,
            IMapper mapper,
            ISchedulesRepository schedulesRepository,
            IEntityUpdateUtility entityUpdateUtility
        )
        {
            _datedShiftsRepository = datedShiftsRepository;
            _mapper = mapper;
            _schedulesRepository = schedulesRepository;
            _entityUpdateUtility = entityUpdateUtility;
        }

        public async Task<DatedShiftDto?> GetAsync(
            string id,
            CancellationToken cancellation = default
        )
        {
            var entity = await _datedShiftsRepository.GetAsync(
                new EntityRequest<DatedShiftEntity> { EntityId = id }
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
            var pageIndex = request.PageIndex.Value;
            var amount = request.Amount.Value;

            var repoRequest = new PaginatedDatedShiftRequest
            {
                Amount = amount,
                PageIndex = pageIndex
            };

            var entities = await _datedShiftsRepository.GetManyAsync(repoRequest, cancellation);

            var dtos = _mapper.Map<List<DatedShiftDto>>(entities);
            return dtos;
        }

        public async Task<List<DatedShiftDto>> GetForScheduleAsync(
            string scheduleId,
            PaginatedRequest request,
            CancellationToken cancellation = default
        )
        {
            var repoRequest = new PaginatedDatedShiftRequest(scheduleId)
            {
                PageIndex = request.PageIndex.Value,
                Amount = request.Amount.Value,
            };

            var entities = await _datedShiftsRepository.GetManyAsync(repoRequest, cancellation);
            var dtos = _mapper.Map<List<DatedShiftDto>>(entities);

            return dtos;
        }

        public async Task<ServiceResult> CreateAsync(
            DatedShiftDto dto,
            string scheduleId,
            CancellationToken cancellation = default
        )
        {
            var request = new ScheduleRequest { EntityId = scheduleId };
            var schedule = await _schedulesRepository.GetAsync(request, cancellation);

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
            var source = _mapper.Map<DatedShiftEntity>(dto);

            var request = new EntityRequest<DatedShiftEntity> { EntityId = source.Id };

            var target = await _datedShiftsRepository.GetAsync(request, cancellation);

            if (target == null)
            {
                return ServiceResult.Error($"Датированная смена (ID: {source.Id}) не была найдена");
            }

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
            var result = await _datedShiftsRepository.DeleteAsync(id, cancellation);

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
    }
}
