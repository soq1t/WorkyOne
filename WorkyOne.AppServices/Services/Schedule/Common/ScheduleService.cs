using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Users;
using WorkyOne.AppServices.Interfaces.Services;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Utilities;
using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Requests.Schedule.Common;
using WorkyOne.Domain.Requests.Users;
using ContractRequest = WorkyOne.Contracts.Repositories.Requests.Schedule.Common;

namespace WorkyOne.AppServices.Services.Schedule.Common
{
    /// <summary>
    /// Сервис по работе с расписанием
    /// </summary>
    public sealed class ScheduleService : IScheduleService
    {
        private readonly ISchedulesRepository _schedulesRepository;
        private readonly IUserDatasRepository _userDatasRepository;
        private readonly IMapper _mapper;

        private readonly IEntityUpdateUtility _entityUpdateUtility;

        public ScheduleService(
            ISchedulesRepository schedulesRepository,
            IUserDatasRepository userDatasRepository,
            IMapper mapper,
            IEntityUpdateUtility entityUpdateUtility
        )
        {
            _schedulesRepository = schedulesRepository;
            _userDatasRepository = userDatasRepository;
            _mapper = mapper;
            _entityUpdateUtility = entityUpdateUtility;
        }

        public async Task<string?> CreateScheduleAsync(
            string scheduleName,
            string userDataId,
            CancellationToken cancellation = default
        )
        {
            var userData = await _userDatasRepository.GetAsync(
                new UserDataRequest { EntityId = userDataId },
                cancellation
            );

            if (userData == null)
            {
                return null;
            }

            if (cancellation.IsCancellationRequested)
            {
                return null;
            }
            var schedule = new ScheduleEntity() { UserDataId = userDataId, Name = scheduleName, };

            var result = await _schedulesRepository.CreateAsync(schedule, cancellation);

            if (result.IsSuccess)
            {
                await _schedulesRepository.SaveChangesAsync(cancellation);
                return schedule.Id;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteSchedulesAsync(
            List<string> schedulesIds,
            CancellationToken cancellation = default
        )
        {
            var result = await _schedulesRepository.DeleteManyAsync(schedulesIds, cancellation);

            if (cancellation.IsCancellationRequested)
            {
                return false;
            }

            if (result.IsSuccess)
            {
                await _schedulesRepository.SaveChangesAsync(cancellation);
            }

            return result.IsSuccess;
        }

        public async Task<ScheduleDto?> GetAsync(
            string scheduleId,
            CancellationToken cancellation = default
        )
        {
            var request = new ScheduleRequest(scheduleId, true);

            var item = await _schedulesRepository.GetAsync(request, cancellation);

            if (item == null)
            {
                return null;
            }
            else
            {
                return _mapper.Map<ScheduleDto>(item);
            }
        }

        public async Task<List<ScheduleDto>> GetByUserDataAsync(
            string userDataId,
            CancellationToken cancellation = default
        )
        {
            var request = new PaginatedScheduleRequest(userDataId);

            var entities = await _schedulesRepository.GetManyAsync(request, cancellation);

            var dtos = _mapper.Map<List<ScheduleDto>>(entities);
            return dtos;
        }

        public async Task<List<ScheduleDto>> GetManyAsync(
            ContractRequest.PaginatedScheduleRequest request,
            CancellationToken cancellation = default
        )
        {
            var repoRequest = new PaginatedScheduleRequest
            {
                PageIndex = request.PageIndex.Value,
                Amount = request.Amount.Value,
                IncludeShifts = request.IncludeFullData.Value,
                IncludeTemplate = request.IncludeFullData.Value,
            };

            var entities = await _schedulesRepository.GetManyAsync(repoRequest, cancellation);

            var dtos = _mapper.Map<List<ScheduleDto>>(entities);

            return dtos;
        }

        public async Task<bool> UpdateScheduleAsync(
            ScheduleDto scheduleDto,
            CancellationToken cancellation = default
        )
        {
            ScheduleEntity source = _mapper.Map<ScheduleEntity>(scheduleDto);

            var target = await _schedulesRepository.GetAsync(
                new ScheduleRequest { EntityId = source.Id }
            );

            if (target == null)
            {
                return false;
            }

            _entityUpdateUtility.Update(target, source);

            var result = _schedulesRepository.Update(target);

            if (result.IsSuccess)
            {
                await _schedulesRepository.SaveChangesAsync(cancellation);
            }

            return result.IsSuccess;
        }
    }
}
