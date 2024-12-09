using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using WorkyOne.AppServices.Interfaces.Services.Auth;
using WorkyOne.Contracts.Services.Requests;
using WorkyOne.Domain.Entities.Users;

namespace WorkyOne.AppServices.Services.Auth
{
    /// <summary>
    /// Cервиса, отвечающий за авторизацию и аутентификацию пользователей
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IJwtService _jwtService;
        private readonly ISessionService _sessionService;
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthService(
            UserManager<UserEntity> userManager,
            IJwtService jwtService,
            ISessionService sessionService,
            IHttpContextAccessor contextAccessor
        )
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _sessionService = sessionService;
            _contextAccessor = contextAccessor;
        }

        public bool IsAuthenticated()
        {
            var user = _contextAccessor.HttpContext.User;

            if (user == null)
            {
                return false;
            }

            if (user.Identity == null)
            {
                return false;
            }

            return user.Identity.IsAuthenticated;
        }

        public bool IsUserInRoles(params string[] roles)
        {
            var user = _contextAccessor.HttpContext.User;

            return IsUserInRoles(user, roles);
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

        public async Task<SignInResult> LogInAsync(
            LogInRequest request,
            CancellationToken cancellation = default
        )
        {
            var user = await _userManager.FindByNameAsync(request.Username);

            if (
                user == null
                || !await _userManager.CheckPasswordAsync(user, request.Password)
                || !user.IsActivated
            )
            {
                return SignInResult.Failed;
            }

            if (request.CreateSession)
            {
                var sessionToken = await _sessionService.CreateSessionAsync(user.Id, cancellation);

                if (sessionToken == null)
                {
                    return SignInResult.Failed;
                }

                _sessionService.WriteTokenToCookies(sessionToken);
            }

            var jwtToken = await _jwtService.GenerateJwtTokenAsync(user.Id, cancellation);

            if (jwtToken == null)
            {
                return SignInResult.Failed;
            }

            _jwtService.WriteToCookies(jwtToken);

            return SignInResult.Success;
        }

        public Task LogOutAsync(CancellationToken cancellation = default)
        {
            _jwtService.ClearCookies();
            return _sessionService.DeleteCurrentSessionAsync(cancellation);
        }

        public Task<IdentityResult> RegisterAsync(
            RegistrationRequest request,
            CancellationToken cancellation = default
        )
        {
            var user = new UserEntity()
            {
                UserName = request.Username,
                IsActivated = false,
                FirstName = request.FirstName
            };

            return _userManager.CreateAsync(user, request.Password);
        }
    }
}
