using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Shifts.Basic;
using WorkyOne.AppServices.Interfaces.Repositories.Users;
using WorkyOne.AppServices.Interfaces.Services;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services.Users;
using WorkyOne.AppServices.Interfaces.Stores;
using WorkyOne.AppServices.Interfaces.Utilities;
using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Contracts.DTOs.Schedule.Shifts.Basic;
using WorkyOne.Contracts.Enums.Result;
using WorkyOne.Contracts.Repositories.Result;
using WorkyOne.Contracts.Services.CreateModels.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Shifts.Basic;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Domain.Requests.Schedule.Common;
using WorkyOne.Domain.Requests.Users;
using WorkyOne.Domain.Specifications.AccesFilters.Abstractions;
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
        private readonly IPersonalShiftRepository _personalShiftRepository;
        private readonly IUserDatasRepository _userDatasRepository;
        private readonly IDailyInfosRepository _dailyInfosRepository;

        private readonly IMapper _mapper;
        private readonly IEntityUpdateUtility _entityUpdateUtility;
        private readonly IAccessFiltersStore _accessFiltersStore;

        private readonly IDateTimeService _dateTimeService;
        private readonly ITemplateService _templateService;
        private readonly IWorkGraphicService _workGraphicService;

        private AccessFilter<ScheduleEntity> _scheduleAccessFilter =>
            _accessFiltersStore.GetFilter<ScheduleEntity>();

        private AccessFilter<UserDataEntity> _userDataAccessFilter =>
            _accessFiltersStore.GetFilter<UserDataEntity>();

        public ScheduleService(
            ISchedulesRepository schedulesRepository,
            IUserDatasRepository userDatasRepository,
            IMapper mapper,
            IEntityUpdateUtility entityUpdateUtility,
            IUserAccessInfoProvider accessInfoProvider,
            IDailyInfosRepository dailyInfosRepository,
            ISharedShiftsRepository sharedShiftsRepository,
            IDateTimeService dateTimeService,
            ITemplateService templateService,
            IAccessFiltersStore accessFiltersStore,
            IWorkGraphicService workGraphicService,
            IPersonalShiftRepository personalShiftRepository
        )
        {
            _schedulesRepository = schedulesRepository;
            _userDatasRepository = userDatasRepository;
            _sharedShiftsRepository = sharedShiftsRepository;

            _mapper = mapper;
            _entityUpdateUtility = entityUpdateUtility;
            _dailyInfosRepository = dailyInfosRepository;
            _dateTimeService = dateTimeService;
            _templateService = templateService;
            _accessFiltersStore = accessFiltersStore;
            _workGraphicService = workGraphicService;
            _personalShiftRepository = personalShiftRepository;
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
            schedule.Template.StartDate = DateOnly.FromDateTime(_dateTimeService.GetNow());

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
            var request = new ScheduleRequest(filter)
            {
                IncludeTemplate = true,
                IncludeShifts = true
            };

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

            var result = await _templateService.UpdateAsync(
                target.Template,
                source.Template,
                cancellation
            );

            if (!result.IsSucceed)
            {
                return result;
            }

            result = _personalShiftRepository.Renew(target.PersonalShifts, source.PersonalShifts);

            if (!result.IsSucceed)
            {
                return result;
            }

            _entityUpdateUtility.Update(target, source);

            result = _schedulesRepository.Update(target);

            if (result.IsSucceed)
            {
                await _schedulesRepository.SaveChangesAsync(cancellation);

                var now = DateTime.Now;

                var startDate = new DateOnly(now.Year - 1, 1, 1);
                var endDate = new DateOnly(now.Year + 1, 12, 31);

                await _workGraphicService.CreateAsync(
                    new WorkGraphicModel
                    {
                        ScheduleId = target.Id,
                        StartDate = startDate,
                        EndDate = endDate
                    }
                );
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
    }
}
