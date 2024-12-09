using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.DTOs.Common;

namespace WorkyOne.MVC.Models.Admin
{
    /// <summary>
    /// Модель для страницы редактирования пользователя
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// Информация о пользователе
        /// </summary>
        [Required]
        public UserInfoDto User { get; set; }

        /// <summary>
        /// Количество расписаний у пользователя
        /// </summary>
        public int SchedulesAmount { get; set; }

        /// <summary>
        /// Список ролей в приложении
        /// </summary>
        public List<string> Roles { get; set; } = [];
    }
}
