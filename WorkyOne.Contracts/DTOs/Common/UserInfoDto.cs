using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        [DisplayName("Имя пользователя")]
        [Required(ErrorMessage = "Введите имя пользователя")]
        public string UserName { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        [DisplayName("Имя")]
        [Required(ErrorMessage = "Введите имя")]
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
        /// Активирован ли пользователь
        /// </summary>
        [DisplayName("Статус активации")]
        public bool IsActivated { get; set; }

        /// <summary>
        /// Идентификатор избранного расписания
        /// </summary>
        public string? FavoriteScheduleId { get; set; }

        /// <summary>
        /// Роли пользователя
        /// </summary>
        public List<string> Roles { get; set; } = [];
    }
}
