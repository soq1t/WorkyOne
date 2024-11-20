using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Repositories.Users;
using WorkyOne.AppServices.Interfaces.Services.Users;
using WorkyOne.AppServices.Interfaces.Stores;
using WorkyOne.Contracts.DTOs.Common;
using WorkyOne.Contracts.Repositories.Result;
using WorkyOne.Contracts.Services.Common;
using WorkyOne.Contracts.Services.GetRequests.Users;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Domain.Interfaces.Specification;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Domain.Requests.Schedule.Common;
using WorkyOne.Domain.Requests.Users;
using WorkyOne.Domain.Specifications.AccesFilters.Abstractions;
using WorkyOne.Domain.Specifications.AccesFilters.Users;
using WorkyOne.Domain.Specifications.Base;
using WorkyOne.Domain.Specifications.Common;

namespace WorkyOne.AppServices.Services.Users
{
    /// <summary>
    /// Сервис по работе с пользователями
    /// </summary>
    public sealed class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepo;
        private readonly ISchedulesRepository _schedulesRepository;
        private readonly IUserDatasRepository _userDatasRepo;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        private readonly IAccessFiltersStore _accessFiltersStore;

        public UsersService(
            IUsersRepository usersRepo,
            IUserDatasRepository userDatasRepo,
            IMapper mapper,
            IHttpContextAccessor contextAccessor,
            IUserAccessInfoProvider accessInfoProvider,
            ISchedulesRepository schedulesRepository,
            IAccessFiltersStore accessFiltersStore
        )
        {
            _usersRepo = usersRepo;
            _userDatasRepo = userDatasRepo;
            _mapper = mapper;
            _contextAccessor = contextAccessor;

            _schedulesRepository = schedulesRepository;
            _accessFiltersStore = accessFiltersStore;
        }

        public async Task<UserInfoDto?> GetUserInfoAsync(
            UserInfoRequest request,
            CancellationToken cancellation = default
        )
        {
            ISpecification<UserEntity> filter;

            if (request.IsCurrentUserRequired)
            {
                var username = _contextAccessor.HttpContext.User?.Identity?.Name;
                filter = new Specification<UserEntity>(x => x.UserName == username);
            }
            else
            {
                filter = new Specification<UserEntity>(x =>
                    x.Id == request.UserId || x.UserName == request.UserName
                ).And(_accessFiltersStore.GetFilter<UserEntity>());
            }

            var user = await _usersRepo.GetAsync(
                new EntityRequest<UserEntity>(filter),
                cancellation
            );

            if (user == null)
            {
                if (user == null)
                    return null;
            }

            var userDataFilter = new Specification<UserDataEntity>(x => x.UserId == user.Id);

            UserDataEntity? userData = await _userDatasRepo.GetAsync(
                new UserDataRequest(userDataFilter)
                {
                    IncludeFullSchedulesInfo = request.IncludeFullSchedulesInfo,
                    IncludeSchedules = request.IncludeSchedules,
                },
                cancellation
            );

            if (userData == null)
            {
                userData = new UserDataEntity() { UserId = user.Id };

                var result = await _userDatasRepo.CreateAsync(userData, cancellation);

                if (result.IsSucceed)
                {
                    await _userDatasRepo.SaveChangesAsync(cancellation);
                }
                else
                {
                    return null;
                }
            }

            var dto = _mapper.Map<UserInfoDto>(user);
            _mapper.Map(userData, dto);

            return dto;
        }

        public async Task<ServiceResult> SetFavoriteScheduleAsync(
            string userDataId,
            string scheduleId,
            CancellationToken cancellation = default
        )
        {
            var userData = await _userDatasRepo.GetAsync(
                new UserDataRequest(
                    new EntityIdFilter<UserDataEntity>(userDataId).And(
                        _accessFiltersStore.GetFilter<UserDataEntity>()
                    )
                )
                {
                    IncludeSchedules = true
                },
                cancellation
            );

            if (userData == null)
            {
                return ServiceResult.Error("UserData not found or inaccessible");
            }

            RepositoryResult? result;

            if (userData.FavoriteScheduleId == scheduleId)
            {
                userData.FavoriteScheduleId = null;
                userData.FavoriteSchedule = null;

                result = _userDatasRepo.Update(userData);

                if (result.IsSucceed)
                {
                    await _userDatasRepo.SaveChangesAsync(cancellation);

                    if (cancellation.IsCancellationRequested)
                    {
                        return ServiceResult.CancellationRequested();
                    }
                    else
                    {
                        return ServiceResult.Ok("Success");
                    }
                }
                else
                {
                    return ServiceResult.Error("Error");
                }
            }

            var schedule = userData.Schedules.FirstOrDefault(x => x.Id == scheduleId);

            if (schedule == null)
            {
                return ServiceResult.Error("Schedule not found");
            }

            userData.FavoriteSchedule = schedule;
            userData.FavoriteScheduleId = schedule.Id;
            result = _userDatasRepo.Update(userData);

            if (result.IsSucceed)
            {
                await _userDatasRepo.SaveChangesAsync(cancellation);

                if (cancellation.IsCancellationRequested)
                {
                    return ServiceResult.CancellationRequested();
                }

                return ServiceResult.Ok("Success");
            }
            else
            {
                return ServiceResult.Error("Error");
            }
        }
    }
}
