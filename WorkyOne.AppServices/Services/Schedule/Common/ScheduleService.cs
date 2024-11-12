using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts.Basic;
using WorkyOne.AppServices.Interfaces.Repositories.Users;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services.Users;
using WorkyOne.AppServices.Interfaces.Utilities;
using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Contracts.DTOs.Schedule.Shifts.Basic;
using WorkyOne.Contracts.Enums.Result;
using WorkyOne.Contracts.Repositories.Result;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Shifts.Basic;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Domain.Requests.Schedule.Common;
using WorkyOne.Domain.Requests.Users;
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
        private readonly ISharedShiftsRepository _sharedShiftsRepository;
        private readonly IUserDatasRepository _userDatasRepository;
        private readonly IDailyInfosRepository _dailyInfosRepository;
        private readonly IMapper _mapper;
        private readonly IEntityUpdateUtility _entityUpdateUtility;

        private ScheduleAccessFilter _scheduleAccessFilter;
        private UserDataAccessFilter _userDataAccessFilter;

        public ScheduleService(
            ISchedulesRepository schedulesRepository,
            IUserDatasRepository userDatasRepository,
            IMapper mapper,
            IEntityUpdateUtility entityUpdateUtility,
            IUserAccessInfoProvider accessInfoProvider,
            IDailyInfosRepository dailyInfosRepository,
            ISharedShiftsRepository sharedShiftsRepository
        )
        {
            _schedulesRepository = schedulesRepository;
            _userDatasRepository = userDatasRepository;
            _sharedShiftsRepository = sharedShiftsRepository;

            _mapper = mapper;
            _entityUpdateUtility = entityUpdateUtility;

            InitAccessFiltersAsync(accessInfoProvider).Wait();
            _dailyInfosRepository = dailyInfosRepository;
        }

        public async Task<RepositoryResult> CreateScheduleAsync(
            ScheduleDto dto,
            CancellationToken cancellation = default
        )
        {
            var filter = new EntityIdFilter<UserDataEntity>(dto.UserDataId).And(
                _userDataAccessFilter
            );

            var userData = await _userDatasRepository.GetAsync(
                new UserDataRequest(filter),
                cancellation
            );

            if (userData == null)
            {
                return RepositoryResult.Error(
                    ResultType.NotFound,
                    dto.UserDataId,
                    nameof(UserDataEntity)
                );
            }

            if (cancellation.IsCancellationRequested)
            {
                return RepositoryResult.CancellationRequested();
            }
            var schedule = _mapper.Map<ScheduleEntity>(dto);
            schedule.Id = Guid.NewGuid().ToString();

            var result = await _schedulesRepository.CreateAsync(schedule, cancellation);

            if (result.IsSucceed)
            {
                await _schedulesRepository.SaveChangesAsync(cancellation);
            }

            return result;
        }

        public async Task<RepositoryResult> DeleteAsync(
            string id,
            CancellationToken cancellation = default
        )
        {
            var filter = new EntityIdFilter<ScheduleEntity>(id).And(_scheduleAccessFilter);

            var deleted = await _schedulesRepository.GetAsync(
                new ScheduleRequest(filter),
                cancellation
            );

            if (cancellation.IsCancellationRequested)
            {
                return RepositoryResult.CancellationRequested();
            }

            if (deleted == null)
            {
                return RepositoryResult.Error(ResultType.NotFound, id, nameof(ScheduleEntity));
            }

            var result = _schedulesRepository.Delete(deleted);

            if (result.IsSucceed)
            {
                await _schedulesRepository.SaveChangesAsync(cancellation);

                if (cancellation.IsCancellationRequested)
                {
                    return RepositoryResult.CancellationRequested();
                }

                await _dailyInfosRepository.DeleteByConditionAsync(
                    new Specification<DailyInfoEntity>(x => x.ScheduleId == deleted.Id)
                );
            }

            return result;
        }

        public async Task<ScheduleDto?> GetAsync(
            string scheduleId,
            CancellationToken cancellation = default
        )
        {
            var filter = new EntityIdFilter<ScheduleEntity>(scheduleId).And(_scheduleAccessFilter);
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
            await AddSharedShiftsAsync(cancellation, dto);

            return dto;
        }

        public async Task<List<ScheduleDto>> GetByUserAsync(
            string userId,
            Contract.PaginatedScheduleRequest request,
            CancellationToken cancellation = default
        )
        {
            var filter = new Specification<UserDataEntity>(x => x.UserId == userId).And(
                _userDataAccessFilter
            );

            var userData = await _userDatasRepository.GetAsync(
                new UserDataRequest(filter),
                cancellation
            );

            if (userData == null)
            {
                return [];
            }

            return await GetByUserDataAsync(userData.Id, request, cancellation);
        }

        public async Task<List<ScheduleDto>> GetByUserDataAsync(
            string userDataId,
            Contract.PaginatedScheduleRequest request,
            CancellationToken cancellation = default
        )
        {
            var filter = new Specification<ScheduleEntity>(x => x.UserDataId == userDataId).And(
                _scheduleAccessFilter
            );
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

            if (request.IncludeFullData)
            {
                await AddSharedShiftsAsync(cancellation, dtos.ToArray());
            }
            return dtos;
        }

        public async Task<List<ScheduleDto>> GetManyAsync(
            Contract.PaginatedScheduleRequest request,
            CancellationToken cancellation = default
        )
        {
            var repoRequest = new PaginatedScheduleRequest(
                _scheduleAccessFilter,
                request.PageIndex,
                request.Amount
            )
            {
                IncludeShifts = request.IncludeFullData,
                IncludeTemplate = request.IncludeFullData,
            };

            var entities = await _schedulesRepository.GetManyAsync(repoRequest, cancellation);

            var dtos = _mapper.Map<List<ScheduleDto>>(entities);

            if (request.IncludeFullData)
            {
                await AddSharedShiftsAsync(cancellation, dtos.ToArray());
            }

            return dtos;
        }

        public async Task<RepositoryResult> UpdateScheduleAsync(
            ScheduleDto scheduleDto,
            CancellationToken cancellation = default
        )
        {
            var filter = new EntityIdFilter<ScheduleEntity>(scheduleDto.Id).And(
                _scheduleAccessFilter
            );
            var request = new ScheduleRequest(filter) { IncludeTemplate = true };

            var target = await _schedulesRepository.GetAsync(request, cancellation);

            if (target == null)
            {
                return RepositoryResult.Error(
                    ResultType.NotFound,
                    scheduleDto.Id,
                    nameof(ScheduleEntity)
                );
            }

            ScheduleEntity source = _mapper.Map<ScheduleEntity>(scheduleDto);

            _entityUpdateUtility.Update(target, source);

            var result = _schedulesRepository.Update(target);

            if (result.IsSucceed)
            {
                await _schedulesRepository.SaveChangesAsync(cancellation);
            }
            return result;
        }

        private async Task AddSharedShiftsAsync(
            CancellationToken cancellation = default,
            params ScheduleDto[] schedules
        )
        {
            if (schedules.Length > 0)
            {
                var sharedShifts = await _sharedShiftsRepository.GetManyAsync(
                    new PaginatedRequest<SharedShiftEntity>(
                        new Specification<SharedShiftEntity>(x => true),
                        1,
                        50
                    ),
                    cancellation
                );

                var shiftsDtos = _mapper.Map<List<SharedShiftDto>>(sharedShifts);

                foreach (var schedule in schedules)
                {
                    schedule.SharedShifts = shiftsDtos;
                }
            }
        }

        private async Task InitAccessFiltersAsync(IUserAccessInfoProvider provider)
        {
            var accessInfo = await provider.GetCurrentAsync();

            _scheduleAccessFilter = new ScheduleAccessFilter(accessInfo);
            _userDataAccessFilter = new UserDataAccessFilter(accessInfo);
        }
    }
}
