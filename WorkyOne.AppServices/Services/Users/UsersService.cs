using System.Security.Claims;
using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Users;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Users;
using WorkyOne.Contracts.DTOs.Common;
using WorkyOne.Contracts.Services.GetRequests.Users;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Domain.Requests.Users;
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
        private readonly IUserDatasRepository _userDatasRepo;
        private readonly IMapper _mapper;
        private readonly IUserAccessInfoProvider _accessInfoProvider;

        public UsersService(
            IUsersRepository usersRepo,
            IUserDatasRepository userDatasRepo,
            IMapper mapper,
            IUserAccessInfoProvider accessInfoProvider
        )
        {
            _usersRepo = usersRepo;
            _userDatasRepo = userDatasRepo;
            _mapper = mapper;
            _accessInfoProvider = accessInfoProvider;
        }

        public async Task<UserInfoDto?> GetUserInfoAsync(
            UserInfoRequest request,
            CancellationToken cancellation = default
        )
        {
            var accessInfo = await _accessInfoProvider.GetCurrentAsync(cancellation);

            if (accessInfo.UserId != request.UserId)
            {
                if (!accessInfo.IsAdmin)
                {
                    return null;
                }
            }

            var user = await _usersRepo.GetAsync(
                new EntityRequest<UserEntity>(new EntityIdFilter<UserEntity>(request.UserId)),
                cancellation
            );

            if (user == null)
            {
                user = await _usersRepo.GetAsync(
                    new EntityRequest<UserEntity>(
                        new Specification<UserEntity>(x => x.UserName == request.UserName)
                    ),
                    cancellation
                );

                if (user == null)
                    return null;
            }

            UserDataEntity? userData = await _userDatasRepo.GetAsync(
                new UserDataRequest(new Specification<UserDataEntity>(x => x.UserId == user.Id))
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

        public bool IsUserInRoles(ClaimsPrincipal user, params string[] roles)
        {
            if (user == null)
                return false;

            if (user.IsInRole("God"))
                return true;

            foreach (var item in roles)
            {
                if (user.IsInRole(item))
                    return true;
            }

            return false;
        }
    }
}
