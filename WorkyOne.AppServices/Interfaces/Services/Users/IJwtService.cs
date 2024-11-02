namespace WorkyOne.AppServices.Interfaces.Services.Users
{
    /// <summary>
    /// Интерфейс сервиса, отвечающего за JWT токены
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// Возвращает JWT токен для пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        public Task<string?> GenerateJwtTokenAsync(string username, string password);
    }
}
