using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.MVC.Models.Schedule;

namespace WorkyOne.MVC.Models.Common
{
    /// <summary>
    /// Вью модель для главной страницы
    /// </summary>
    public class HomeViewModel
    {
        /// <summary>
        /// Вью модель календаря
        /// </summary>
        [Required]
        public CalendarViewModel CalendarViewModel { get; set; }

        /// <summary>
        /// Список раписаний пользователя
        /// </summary>
        public List<ScheduleDto>? Schedules { get; set; }

        /// <summary>
        /// Идентификатор пользовательских данных
        /// </summary>
        public string? UserDataId { get; set; }
    }
}
