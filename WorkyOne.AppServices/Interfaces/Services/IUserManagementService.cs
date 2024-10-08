using WorkyOne.Contracts.DTOs;

namespace WorkyOne.AppServices.Interfaces.Services
{
    /// <summary>
    /// Интерфейс сервиса управления пользователями
    /// </summary>
    public interface IUserManagementService
    {
        /// <summary>
        /// Возвращает данные пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        public Task<UserInfoDto?> GetUserInfoAsync(string userId);
    }
}
