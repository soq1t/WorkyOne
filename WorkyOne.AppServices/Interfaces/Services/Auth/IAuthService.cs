using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using WorkyOne.Contracts.Services.Requests;

namespace WorkyOne.AppServices.Interfaces.Services.Auth
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

        /// <summary>
        /// Совершает вход в аккаунт с указанными данными
        /// </summary>
        /// <param name="request">Запрос с данными на вход в аккаунт</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<SignInResult> LogInAsync(
            LogInRequest request,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Выходит из текущего аккаунта
        /// </summary>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task LogOutAsync(CancellationToken cancellation = default);
    }
}
