using System.Security.Claims;

namespace WorkyOne.AppServices.Interfaces.Services.Users
{
    /// <summary>
    /// Интерфейс сервиса, отвечающего за авторизацию и аутентификацию пользователей
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Проверяет, состоит ли пользователь в определённых ролях
        /// </summary>
        /// <param name="user">Проверяемый пользователь</param>
        /// <param name="roles">Роли, которые проверяются для пользователя</param>
        public bool IsUserInRoles(ClaimsPrincipal user, params string[] roles);
    }
}
