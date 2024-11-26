using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.DTOs.Schedule.Common;

namespace WorkyOne.MVC.Models.Schedule
{
    /// <summary>
    /// Модель для расписания
    /// </summary>
    public class ScheduleViewModel
    {
        /// <summary>
        /// Адрес, откуда пришёл запрос
        /// </summary>
        [Required]
        public string Referer { get; set; }

        /// <summary>
        /// DTO расписания
        /// </summary>
        [Required]
        public ScheduleDto Schedule { get; set; }
    }
}
