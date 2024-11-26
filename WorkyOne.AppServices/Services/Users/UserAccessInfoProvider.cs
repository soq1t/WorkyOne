using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
        private readonly IDistributedCache _cache;
        private readonly IJwtService _jwtService;

        private readonly UserManager<UserEntity> _userManager;
        private readonly IAuthService _authService;

        public UserAccessInfoProvider(
            IHttpContextAccessor contextAccessor,
            IUserDatasRepository userDataRepo,
            UserManager<UserEntity> userManager,
            IAuthService authService,
            IDistributedCache cache,
            IJwtService jwtService
        )
        {
            _contextAccessor = contextAccessor;
            _userDataRepo = userDataRepo;
            _userManager = userManager;
            _authService = authService;
            _cache = cache;
            _jwtService = jwtService;
        }

        public async Task<UserAccessInfo> GetCurrentAsync(CancellationToken cancellation = default)
        {
            var key = _jwtService.GetFromCookies() ?? "non-authorized";
            var userAccessInfo = await GetFromCache(key);

            if (userAccessInfo != null)
            {
                return userAccessInfo;
            }

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

            userAccessInfo = new UserAccessInfo(userData.Id, user.Id, isAdmin);
            await SaveToCache(key, userAccessInfo);
            return userAccessInfo;
        }

        private async Task SaveToCache(string key, UserAccessInfo userAccessInfo)
        {
            var json = JsonConvert.SerializeObject(userAccessInfo);

            await _cache.SetStringAsync(
                key,
                json,
                new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                }
            );
        }

        private async Task<UserAccessInfo?> GetFromCache(string key)
        {
            var json = await _cache.GetStringAsync(key);

            if (json == null)
            {
                return null;
            }
            else
            {
                return JsonConvert.DeserializeObject<UserAccessInfo>(json);
            }
        }
    }
}
