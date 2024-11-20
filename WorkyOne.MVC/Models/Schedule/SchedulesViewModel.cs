using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.DTOs.Schedule.Common;

namespace WorkyOne.MVC.Models.Schedule
{
    /// <summary>
    /// Вью модель для расписаний
    /// </summary>
    public class SchedulesViewModel
    {
        /// <summary>
        /// Список расписаний
        /// </summary>
        [Required]
        public List<ScheduleDto> Schedules { get; set; } = new List<ScheduleDto>();

        /// <summary>
        /// Идентификатор "любимого" расписания
        /// </summary>
        public string? FavoriteScheduleId { get; set; }
    }
}
