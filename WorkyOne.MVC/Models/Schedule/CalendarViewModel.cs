using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.DTOs.Schedule.Common;

namespace WorkyOne.MVC.Models.Schedule
{
    /// <summary>
    /// Вью модель для календаря с расписанием
    /// </summary>
    public class CalendarViewModel
    {
        /// <summary>
        /// Год
        /// </summary>
        [Range(2000, 3000)]
        public int Year { get; set; }

        /// <summary>
        /// Порядковый номер месяца
        /// </summary>
        [Range(1, 12)]
        public int Month { get; set; }

        /// <summary>
        /// Название месяца
        /// </summary>
        [Required]
        public string MonthName { get; set; }

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
