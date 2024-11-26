using WorkyOne.AppServices.Interfaces.Services.Auth;
using WorkyOne.AppServices.Interfaces.Services.Common;
using WorkyOne.AppServices.Interfaces.Services.Users;
using WorkyOne.Domain.Specifications.AccesFilters.Common;

namespace WorkyOne.AppServices.Decorators
{
    /// <summary>
    /// Кеширующий декоратор для <see cref="IUserAccessInfoProvider"/>
    /// </summary>
    public class UserAccessInfoCachingDecorator : IUserAccessInfoProvider
    {
        private readonly IUserAccessInfoProvider _userAccessInfoProvider;
        private readonly ICachingService _cachingService;
        private readonly IJwtService _jwtService;

        public UserAccessInfoCachingDecorator(
            IUserAccessInfoProvider userAccessInfoProvider,
            ICachingService cachingService,
            IJwtService jwtService
        )
        {
            _userAccessInfoProvider = userAccessInfoProvider;
            _cachingService = cachingService;
            _jwtService = jwtService;
        }

        public async Task<UserAccessInfo> GetCurrentAsync(CancellationToken cancellation = default)
        {
            string key = _jwtService.GetFromCookies() ?? "non-authorized";

            return await _cachingService.GetAsync(
                key,
                async () => await _userAccessInfoProvider.GetCurrentAsync(cancellation),
                TimeSpan.FromMinutes(5),
                cancellation
            );
        }
    }
}
