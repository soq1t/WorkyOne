using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts;
using WorkyOne.Contracts.DTOs.Schedule.Shifts;
using WorkyOne.Contracts.Requests.Schedule.Common;
using WorkyOne.Contracts.Requests.Schedule.Shifts;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Shifts;

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

        public DatedShiftsService(
            IDatedShiftsRepository datedShiftsRepository,
            IMapper mapper,
            ISchedulesRepository schedulesRepository
        )
        {
            _datedShiftsRepository = datedShiftsRepository;
            _mapper = mapper;
            _schedulesRepository = schedulesRepository;
        }

        public async Task<bool> CreateAsync(
            DatedShiftDto dto,
            string scheduleId,
            CancellationToken cancellation = default
        )
        {
            var scheduleRequest = new ScheduleRequest { Id = scheduleId };
            var schedule = await _schedulesRepository.GetAsync(scheduleRequest, cancellation);

            if (schedule == null || cancellation.IsCancellationRequested)
            {
                return false;
            }

            DatedShiftEntity shift = _mapper.Map<DatedShiftEntity>(dto);
            shift.Schedule = schedule;

            if (cancellation.IsCancellationRequested)
            {
                return false;
            }

            var result = await _datedShiftsRepository.CreateAsync(shift);

            return result.IsSuccess;
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellation = default)
        {
            var result = await _datedShiftsRepository.DeleteAsync(id, cancellation);
            return result.IsSuccess;
        }

        public async Task<bool> DeleteForScheduleAsync(
            string scheduleId,
            CancellationToken cancellation = default
        )
        {
            var request = new DatedShiftRequest { ScheduleId = scheduleId };

            var deleted = await _datedShiftsRepository.GetByScheduleIdAsync(request, cancellation);

            if (cancellation.IsCancellationRequested)
            {
                return false;
            }

            var result = await _datedShiftsRepository.DeleteManyAsync(
                deleted.Select(e => e.Id).ToList(),
                cancellation
            );

            return result.IsSuccess;
        }

        public async Task<bool> DeleteManyAsync(
            ICollection<string> ids,
            CancellationToken cancellation = default
        )
        {
            var result = await _datedShiftsRepository.DeleteManyAsync(ids, cancellation);
            return result.IsSuccess;
        }

        public async Task<DatedShiftDto?> GetAsync(
            string id,
            CancellationToken cancellation = default
        )
        {
            var entity = await _datedShiftsRepository.GetAsync(new DatedShiftRequest { Id = id });

            if (entity == null)
            {
                return null;
            }

            var dto = _mapper.Map<DatedShiftDto>(entity);
            return dto;
        }

        public async Task<List<DatedShiftDto>> GetForScheduleAsync(
            string scheduleId,
            CancellationToken cancellation = default
        )
        {
            var request = new DatedShiftRequest { ScheduleId = scheduleId };

            var entities = await _datedShiftsRepository.GetByScheduleIdAsync(request, cancellation);
            var dtos = _mapper.Map<List<DatedShiftDto>>(entities);

            return dtos;
        }

        public async Task<bool> UpdateAsync(
            DatedShiftDto dto,
            CancellationToken cancellation = default
        )
        {
            var entity = _mapper.Map<DatedShiftEntity>(dto);

            var result = await _datedShiftsRepository.UpdateAsync(entity, cancellation);
            return result.IsSuccess;
        }
    }
}
