namespace WorkyOne.AppServices.Interfaces.Services.Auth
{
    /// <summary>
    /// Интерфейс сервиса, отвечающего за JWT токены
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// Генерирует и возвращает JWT токен для пользователя
        /// </summary>
        /// <param name="username">Юзернейм</param>
        /// <param name="password">Пароль пользователя</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<string?> GenerateJwtTokenAsync(
            string username,
            string password,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Генерирует и возвращает JWT токен для пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<string?> GenerateJwtTokenAsync(
            string userId,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Записывает токен в кукис
        /// </summary>
        /// <param name="token">JWT токен</param>
        public void WriteToCookies(string token);

        /// <summary>
        /// Записывает токен в заголовки запроса
        /// </summary>
        /// <param name="token">JWT токен</param>
        public void WriteToHeaders(string token);

        /// <summary>
        /// Возвращает JWT токен из кукис
        /// </summary>
        public string? GetFromCookies();

        /// <summary>
        /// Удаляет токен из кукис
        /// </summary>
        public void ClearCookies();
    }
}
