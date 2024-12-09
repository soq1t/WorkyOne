using WorkyOne.Contracts.DTOs.Common;
using WorkyOne.Contracts.Services.Common;
using WorkyOne.Contracts.Services.GetRequests.Common;
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

        /// <summary>
        /// Устанавливает "любимое" расписание
        /// </summary>
        /// <param name="userDataId">Идентификатор пользовательских данных</param>
        /// <param name="scheduleId">Идентификатор расписания</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<ServiceResult> SetFavoriteScheduleAsync(
            string userDataId,
            string scheduleId,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Возвращает некоторое количество пользователей из базы данных
        /// </summary>
        /// <param name="request">Пагинированный запрос</param>
        /// <param name="filter">Фильтр</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<PaginatedResponse<List<UserInfoDto>>> GetUsersAsync(
            PaginatedRequest request,
            UserFilter? filter,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Обновляет указанного пользователя
        /// </summary>
        /// <param name="user">Обновляемый пользователь</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<ServiceResult> UpdateUserAsync(
            UserInfoDto user,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Активирует указанного пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<ServiceResult> ActivateUserAsync(
            string id,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Удаляет пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<ServiceResult> DeleteUserAsync(
            string id,
            CancellationToken cancellation = default
        );
    }
}
