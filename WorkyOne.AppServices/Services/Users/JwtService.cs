using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WorkyOne.AppServices.Interfaces.Services.Users;
using WorkyOne.Contracts.Configuration;
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

        public JwtService(IOptions<JwtOptions> jwtOptions, UserManager<UserEntity> userManager)
        {
            _jwtOptions = jwtOptions;
            _userManager = userManager;
        }

        public async Task<string?> GenerateJwtTokenAsync(string username, string password)
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
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
