using WorkyOne.Contracts.DTOs.Common;

namespace WorkyOne.AppServices.Interfaces.Services.Common
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
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<UserInfoDto?> GetUserInfoAsync(
            string userId,
            CancellationToken cancellation = default
        );
    }
}
