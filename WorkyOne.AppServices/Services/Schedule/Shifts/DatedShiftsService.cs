using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts;
using WorkyOne.Contracts.DTOs.Schedule.Shifts;
using WorkyOne.Contracts.Requests.Schedule.Shifts;
using WorkyOne.Domain.Entities.Schedule.Shifts;

namespace WorkyOne.AppServices.Services.Schedule.Shifts
{
    /// <summary>
    /// Сервис по работе с <see cref="DatedShiftEntity"/>
    /// </summary>
    public sealed class DatedShiftsService : IDatedShiftsService
    {
        private readonly IDatedShiftsRepository _repository;
        private readonly IMapper _mapper;

        public DatedShiftsService(IDatedShiftsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> CreateAsync(
            DatedShiftDto dto,
            CancellationToken cancellation = default
        )
        {
            DatedShiftEntity shift = _mapper.Map<DatedShiftEntity>(dto);

            if (cancellation.IsCancellationRequested)
            {
                return false;
            }

            var result = await _repository.CreateAsync(shift);

            return result.IsSuccess;
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellation = default)
        {
            var result = await _repository.DeleteAsync(id, cancellation);
            return result.IsSuccess;
        }

        public async Task<bool> DeleteForScheduleAsync(
            string scheduleId,
            CancellationToken cancellation = default
        )
        {
            var request = new DatedShiftRequest { ScheduleId = scheduleId };

            var deleted = await _repository.GetByScheduleIdAsync(request, cancellation);

            if (cancellation.IsCancellationRequested)
            {
                return false;
            }

            var result = await _repository.DeleteManyAsync(
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
            var result = await _repository.DeleteManyAsync(ids, cancellation);
            return result.IsSuccess;
        }

        public async Task<List<DatedShiftDto>> GetForScheduleAsync(
            string scheduleId,
            CancellationToken cancellation = default
        )
        {
            var request = new DatedShiftRequest { ScheduleId = scheduleId };

            var entities = await _repository.GetByScheduleIdAsync(request, cancellation);
            var dtos = _mapper.Map<List<DatedShiftDto>>(entities);

            return dtos;
        }

        public async Task<bool> UpdateAsync(
            DatedShiftDto dto,
            CancellationToken cancellation = default
        )
        {
            var entity = _mapper.Map<DatedShiftEntity>(dto);

            var result = await _repository.UpdateAsync(entity, cancellation);
            return result.IsSuccess;
        }
    }
}
