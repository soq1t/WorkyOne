using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using WorkyOne.AppServices.Interfaces.Repositories.Users;
using WorkyOne.AppServices.Interfaces.Services.Auth;
using WorkyOne.AppServices.Interfaces.Services.Users;
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
        private readonly IAuthService _authService;
        private readonly ILogger<UserAccessInfoProvider> _logger;

        public UserAccessInfoProvider(
            IHttpContextAccessor contextAccessor,
            IUserDatasRepository userDataRepo,
            UserManager<UserEntity> userManager,
            IAuthService authService,
            ILogger<UserAccessInfoProvider> logger
        )
        {
            _contextAccessor = contextAccessor;
            _userDataRepo = userDataRepo;
            _userManager = userManager;
            _authService = authService;
            _logger = logger;
        }

        public async Task<UserAccessInfo> GetCurrentAsync(CancellationToken cancellation = default)
        {
            //_logger.LogInformation(
            //    "Запрошена информация об уровне доступа для текущего пользователя"
            //);
            if (_contextAccessor.HttpContext == null)
            {
                return new UserAccessInfo(null, null, false);
            }

            ClaimsPrincipal contextUser = _contextAccessor.HttpContext.User;

            if (
                contextUser == null
                || contextUser.Identity == null
                || contextUser.Identity.IsAuthenticated == false
            )
            {
                return new UserAccessInfo(null, null, false);
            }

            var isAdmin = _authService.IsUserInRoles(contextUser, "Admin");

            var user = await _userManager.FindByNameAsync(contextUser.Identity.Name);

            var userData = await _userDataRepo.GetAsync(
                new UserDataRequest(new Specification<UserDataEntity>(x => x.UserId == user.Id))
                {
                    IncludeFullSchedulesInfo = true,
                },
                cancellation
            );

            if (userData == null)
            {
                userData = new UserDataEntity(user.Id);
                await _userDataRepo.CreateAsync(userData, cancellation);
                await _userDataRepo.SaveChangesAsync(cancellation);
            }

            return new UserAccessInfo(userData.Id, user.Id, isAdmin);
        }
    }
}
