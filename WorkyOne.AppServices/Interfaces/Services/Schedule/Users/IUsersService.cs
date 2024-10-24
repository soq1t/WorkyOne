using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Contracts.DTOs.Common;
using WorkyOne.Contracts.Services.GetRequests.Users;

namespace WorkyOne.AppServices.Interfaces.Services.Schedule.Users
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
