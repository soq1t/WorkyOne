using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Shifts;
using WorkyOne.Contracts.DTOs.Schedule.Shifts;
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

        public Task<bool> DeleteAsync(string id, CancellationToken cancellation = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteForScheduleAsync(
            string scheduleId,
            CancellationToken cancellation = default
        )
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteManyAsync(
            IEnumerable<string> ids,
            CancellationToken cancellation = default
        )
        {
            throw new NotImplementedException();
        }

        public Task<List<DatedShiftDto>> GetForScheduleAsync(
            string scheduleId,
            CancellationToken cancellation = default
        )
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(DatedShiftDto dto, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }
    }
}
