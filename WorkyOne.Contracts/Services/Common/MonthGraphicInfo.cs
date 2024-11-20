using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.DTOs.Schedule.Common;

namespace WorkyOne.Contracts.Services.Common
{
    /// <summary>
    /// Информация о рабочем графике на месяц
    /// </summary>
    public class MonthGraphicInfo
    {
        /// <summary>
        /// Информация о календаре на указанный месяц
        /// </summary>
        [Required]
        public CalendarInfo CalendarInfo { get; set; }

        /// <summary>
        /// Расписание, для которого отображается график
        /// </summary>
        public ScheduleDto? Schedule { get; set; }

        /// <summary>
        /// Рабочий график на месяц
        /// </summary>
        public List<DailyInfoDto> Graphic { get; set; } = [];

        /// <summary>
        /// Легенда обозначений смен в текущем графике
        /// </summary>
        public Dictionary<string, string> Legend { get; set; } = [];
    }
}
