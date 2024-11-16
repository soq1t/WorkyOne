using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Contracts.Services.Common;

namespace WorkyOne.MVC.Models.Schedule
{
    /// <summary>
    /// Вью модель для календаря с расписанием
    /// </summary>
    public class CalendarViewModel
    {
        /// <summary>
        /// Информация о календаре
        /// </summary>
        [Required]
        public CalendarInfo Info { get; set; }

        /// <summary>
        /// Рабочий график на текущий месяц
        /// </summary>
        public List<DailyInfoDto>? WorkGraphic { get; set; }

        /// <summary>
        /// Расписание, для которого отображается график
        /// </summary>
        public ScheduleDto? ScheduleDto { get; set; }
    }
}
