using WorkyOne.Domain.Entities.Auth;

namespace WorkyOne.AppServices.Interfaces.Services.Auth
{
    /// <summary>
    /// Интерфейс сервиса управления токенами пользовательскими сессиями
    /// </summary>
    public interface ISessionService
    {
        /// <summary>
        /// Создаёт сессию для указанного пользователя, возвращает токен сессии
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<string?> CreateSessionAsync(
            string userId,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Проверяет действительность токена
        /// </summary>
        /// <param name="token">Сессионный токен</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<bool> VerifyTokenAsync(string token, CancellationToken cancellation = default);

        /// <summary>
        /// Записывает токен в кукис
        /// </summary>
        /// <param name="token">Сессионный токен</param>
        public void WriteTokenToCookies(string token);

        /// <summary>
        /// Возвращает сессионный токен из кукис
        /// </summary>
        public string? GetTokenFromCookies();

        /// <summary>
        /// Обновляет сессионный токен, возвращает обновлённый токен
        /// </summary>
        /// <param name="token">Сессионный токен</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<string?> RefreshTokenAsync(
            string token,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Возвращает сессию по токену сессии
        /// </summary>
        /// <param name="sessionToken">Сессионный токен</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        /// <returns></returns>
        public Task<SessionEntity?> GetSessionByTokenAsync(
            string sessionToken,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Удаляет сессию
        /// </summary>
        /// <param name="cancellation">Токен отмены задачи</param>
        /// <returns></returns>
        public Task DeleteCurrentSessionAsync(CancellationToken cancellation = default);
    }
}
