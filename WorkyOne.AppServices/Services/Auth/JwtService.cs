using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WorkyOne.AppServices.Interfaces.Services;
using WorkyOne.AppServices.Interfaces.Services.Auth;
using WorkyOne.Contracts.Options.Auth;
using WorkyOne.Domain.Entities.Users;

namespace WorkyOne.AppServices.Services.Users
{
    /// <summary>
    /// Сервис, отвечающий за авторизацию и аутентификацию пользователей
    /// </summary>
    public class JwtService : IJwtService
    {
        private readonly IOptions<JwtOptions> _jwtOptions;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDateTimeService _dateTimeService;

        public JwtService(
            IOptions<JwtOptions> jwtOptions,
            UserManager<UserEntity> userManager,
            IHttpContextAccessor httpContextAccessor,
            IDateTimeService dateTimeService
        )
        {
            _jwtOptions = jwtOptions;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _dateTimeService = dateTimeService;
        }

        public void ClearCookies()
        {
            var context = _httpContextAccessor.HttpContext;

            context.Response.Cookies.Delete(_jwtOptions.Value.CookiesName);
        }

        public async Task<string?> GenerateJwtTokenAsync(
            string username,
            string password,
            CancellationToken cancellation = default
        )
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return null;
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);

            if (!isPasswordValid)
            {
                return null;
            }

            return await GenerateJwtTokenAsync(user.Id, cancellation);
        }

        public async Task<string?> GenerateJwtTokenAsync(
            string userId,
            CancellationToken cancellation = default
        )
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Value.Issuer,
                audience: _jwtOptions.Value.Audience,
                claims: claims,
                expires: _dateTimeService.GetNow().AddMinutes(_jwtOptions.Value.ExpirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string? GetFromCookies()
        {
            var context = _httpContextAccessor.HttpContext;

            return context.Request.Cookies[_jwtOptions.Value.CookiesName];
        }

        public void WriteToCookies(string token)
        {
            var context = _httpContextAccessor.HttpContext;

            context.Response.Cookies.Append(
                _jwtOptions.Value.CookiesName,
                token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = _dateTimeService
                        .GetNow()
                        .AddMinutes(_jwtOptions.Value.ExpirationMinutes)
                }
            );
        }

        public void WriteToHeaders(string token)
        {
            var context = _httpContextAccessor.HttpContext;

            context.Request.Headers["Authorization"] = $"Bearer {token}";
        }
    }
}
