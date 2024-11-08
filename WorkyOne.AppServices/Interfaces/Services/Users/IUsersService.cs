using WorkyOne.Contracts.DTOs.Common;
using WorkyOne.Contracts.Services.GetRequests.Users;

namespace WorkyOne.AppServices.Interfaces.Services.Users
{
    /// <summary>
    /// Интерфейс сервиса по работе с пользователями
    /// </summary>
    public interface IUsersService
    {
        /// <summary>
        /// Возвращает <see cref="UserInfoDto"/> для указанного пользователя
        /// </summary>
        /// <param name="request">Запрос, содержащий информацию для получения данных</param>
        /// <param name="cancellation">Токен отмены задачи</param>

        public Task<UserInfoDto?> GetUserInfoAsync(
            UserInfoRequest request,
            CancellationToken cancellation = default
        );
    }
}
