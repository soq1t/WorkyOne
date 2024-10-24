using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Contracts.DTOs.Common;

namespace WorkyOne.Contracts.Services.GetRequests.Users
{
    /// <summary>
    /// Запрос на получение <see cref="UserInfoDto"/>
    /// </summary>
    public class UserInfoRequest
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string? UserName { get; set; }
    }
}
