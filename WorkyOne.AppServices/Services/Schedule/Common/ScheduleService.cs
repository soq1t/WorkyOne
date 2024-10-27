using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Users;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Users;
using WorkyOne.AppServices.Interfaces.Utilities;
using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Contracts.Services.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Domain.Interfaces.Specification;
using WorkyOne.Domain.Requests.Schedule.Common;
using WorkyOne.Domain.Requests.Users;
using WorkyOne.Domain.Specifications.AccesFilters.Common;
using WorkyOne.Domain.Specifications.AccesFilters.Schedule.Common;
using WorkyOne.Domain.Specifications.AccesFilters.Users;
using WorkyOne.Domain.Specifications.Base;
using WorkyOne.Domain.Specifications.Common;
using Contract = WorkyOne.Contracts.Services.GetRequests.Schedule.Common;

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
        private readonly IUserAccessInfoProvider _accessInfoProvider;

        public ScheduleService(
            ISchedulesRepository schedulesRepository,
            IUserDatasRepository userDatasRepository,
            IMapper mapper,
            IEntityUpdateUtility entityUpdateUtility,
            IUserAccessInfoProvider accessInfoProvider
        )
        {
            _schedulesRepository = schedulesRepository;
            _userDatasRepository = userDatasRepository;
            _mapper = mapper;
            _entityUpdateUtility = entityUpdateUtility;
            _accessInfoProvider = accessInfoProvider;
        }

        public async Task<ServiceResult> CreateScheduleAsync(
            ScheduleDto dto,
            CancellationToken cancellation = default
        )
        {
            var accessInfo = await _accessInfoProvider.GetCurrentAsync(cancellation);

            ISpecification<UserDataEntity> filter = new UserDataAccessFilter(accessInfo);
            filter = filter.And(new EntityIdFilter<UserDataEntity>(dto.UserDataId));

            var userData = await _userDatasRepository.GetAsync(
                new UserDataRequest(filter),
                cancellation
            );

            if (userData == null)
            {
                return ServiceResult.Error(
                    $"Не найдены пользовательские данные (ID {dto.UserDataId})"
                );
            }

            if (cancellation.IsCancellationRequested)
            {
                return ServiceResult.CancellationRequested();
            }
            var schedule = _mapper.Map<ScheduleEntity>(dto);
            schedule.Id = Guid.NewGuid().ToString();

            var result = await _schedulesRepository.CreateAsync(schedule, cancellation);

            if (result.IsSuccess)
            {
                await _schedulesRepository.SaveChangesAsync(cancellation);
                return ServiceResult.Ok($"Расписание (ID: {schedule.Id}) успешно создано!");
            }
            else
            {
                return ServiceResult.FromRepositoryResult(result);
            }
        }

        public async Task<ScheduleDto?> GetAsync(
            string scheduleId,
            CancellationToken cancellation = default
        )
        {
            var accessInfo = await _accessInfoProvider.GetCurrentAsync(cancellation);

            ISpecification<ScheduleEntity> filter = new ScheduleAccessFilter(accessInfo);

            filter = filter.And(new EntityIdFilter<ScheduleEntity>(scheduleId));

            var request = new ScheduleRequest(filter)
            {
                IncludeShifts = true,
                IncludeTemplate = true,
            };

            var entity = await _schedulesRepository.GetAsync(request, cancellation);

            if (entity == null)
            {
                return null;
            }

            var dto = _mapper.Map<ScheduleDto>(entity);
            return dto;
        }

        public async Task<List<ScheduleDto>> GetByUserDataAsync(
            string userDataId,
            Contract.PaginatedScheduleRequest request,
            CancellationToken cancellation = default
        )
        {
            var accessInfo = await _accessInfoProvider.GetCurrentAsync(cancellation);
            ISpecification<ScheduleEntity> filter = new ScheduleAccessFilter(accessInfo);
            filter = filter.And(new Specification<ScheduleEntity>(x => x.UserDataId == userDataId));

            var repoRequest = new PaginatedScheduleRequest(
                filter,
                request.PageIndex,
                request.Amount
            )
            {
                IncludeShifts = request.IncludeFullData,
                IncludeTemplate = request.IncludeFullData,
            };

            var entities = await _schedulesRepository.GetManyAsync(repoRequest, cancellation);

            var dtos = _mapper.Map<List<ScheduleDto>>(entities);
            return dtos;
        }

        public async Task<List<ScheduleDto>> GetManyAsync(
            Contract.PaginatedScheduleRequest request,
            CancellationToken cancellation = default
        )
        {
            var accessInfo = await _accessInfoProvider.GetCurrentAsync(cancellation);
            var filter = new ScheduleAccessFilter(accessInfo);

            var repoRequest = new PaginatedScheduleRequest(
                filter,
                request.PageIndex,
                request.Amount
            )
            {
                IncludeShifts = request.IncludeFullData,
                IncludeTemplate = request.IncludeFullData,
            };

            var entities = await _schedulesRepository.GetManyAsync(repoRequest, cancellation);

            var dtos = _mapper.Map<List<ScheduleDto>>(entities);

            return dtos;
        }

        public async Task<ServiceResult> UpdateScheduleAsync(
            ScheduleDto scheduleDto,
            CancellationToken cancellation = default
        )
        {
            var accessInfo = await _accessInfoProvider.GetCurrentAsync(cancellation);
            ISpecification<ScheduleEntity> filter = new ScheduleAccessFilter(accessInfo);

            filter = filter.And(new Specification<ScheduleEntity>(x => x.Id == scheduleDto.Id));

            var request = new ScheduleRequest(filter) { IncludeTemplate = true };

            var target = await _schedulesRepository.GetAsync(request, cancellation);

            if (target == null)
            {
                return ServiceResult.Error(
                    $"Не найдено обновляемое расписание (ID: {scheduleDto.Id})"
                );
            }

            ScheduleEntity source = _mapper.Map<ScheduleEntity>(scheduleDto);

            _entityUpdateUtility.Update(target, source);

            var result = _schedulesRepository.Update(target);

            if (result.IsSuccess)
            {
                await _schedulesRepository.SaveChangesAsync(cancellation);
                return ServiceResult.Ok("Расписание успешно обновлено!");
            }
            else
            {
                return ServiceResult.FromRepositoryResult(result);
            }
        }
    }
}
