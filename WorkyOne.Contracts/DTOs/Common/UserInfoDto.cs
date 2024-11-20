using System.ComponentModel.DataAnnotations.Schema;
using WorkyOne.Contracts.DTOs.Abstractions;
using WorkyOne.Contracts.DTOs.Schedule.Common;

namespace WorkyOne.Contracts.DTOs.Common
{
    /// <summary>
    /// DTO данных о пользователе
    /// </summary>
    public class UserInfoDto : DtoBase
    {
        /// <summary>
        /// Идентификатор данных пользователя
        /// </summary>
        public string UserDataId { get; set; }

        /// <summary>
        /// Пользовательский юзернейм
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Электронная почта пользователя
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Список расписаний пользователя
        /// </summary>
        public List<ScheduleDto> Schedules { get; set; }

        /// <summary>
        /// Идентификатор избранного расписания
        /// </summary>
        public string? FavoriteScheduleId { get; set; }
    }
}
