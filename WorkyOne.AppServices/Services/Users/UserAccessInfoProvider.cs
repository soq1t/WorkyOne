using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using WorkyOne.AppServices.Interfaces.Repositories.Users;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Users;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Domain.Requests.Users;
using WorkyOne.Domain.Specifications.AccesFilters.Common;
using WorkyOne.Domain.Specifications.Base;

namespace WorkyOne.AppServices.Services.Users
{
    /// <summary>
    /// Сервис, предоставляющий <see cref="UserAccessInfo"/>
    /// </summary>
    public sealed class UserAccessInfoProvider : IUserAccessInfoProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUserDatasRepository _userDataRepo;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IUsersService _usersService;

        public UserAccessInfoProvider(
            IHttpContextAccessor contextAccessor,
            IUserDatasRepository userDataRepo,
            UserManager<UserEntity> userManager,
            IUsersService usersService
        )
        {
            _contextAccessor = contextAccessor;
            _userDataRepo = userDataRepo;
            _userManager = userManager;
            _usersService = usersService;
        }

        public async Task<UserAccessInfo> GetCurrentAsync(CancellationToken cancellation = default)
        {
            ClaimsPrincipal contextUser = _contextAccessor.HttpContext.User;

            if (contextUser == null)
            {
                return new UserAccessInfo(null, null, false);
            }

            var isAdmin = _usersService.IsUserInRoles(contextUser, "Admin");

            var user = await _userManager.FindByNameAsync(contextUser.Identity.Name);

            var userData = await _userDataRepo.GetAsync(
                new UserDataRequest(new Specification<UserDataEntity>(x => x.UserId == user.Id))
                {
                    IncludeFullSchedulesInfo = true,
                },
                cancellation
            );

            return new UserAccessInfo(userData.Id, user.Id, isAdmin);
        }
    }
}
