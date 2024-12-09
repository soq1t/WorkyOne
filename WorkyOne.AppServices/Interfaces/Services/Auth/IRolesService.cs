namespace WorkyOne.AppServices.Interfaces.Services.Auth
{
    /// <summary>
    /// Интерфейс сервиса по работе с ролями в приложении
    /// </summary>
    public interface IRolesService
    {
        /// <summary>
        /// Возвращает список ролей в приложении
        /// </summary>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<List<string>> GetRolesAsync(CancellationToken cancellation = default);

        /// <summary>
        /// Устанавливает роли для пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        /// <param name="roles">Список устанавливаемых ролей</param>
        public Task<bool> SetRolesToUserAsync(
            string userId,
            CancellationToken cancellation = default,
            params string[] roles
        );
    }
}
