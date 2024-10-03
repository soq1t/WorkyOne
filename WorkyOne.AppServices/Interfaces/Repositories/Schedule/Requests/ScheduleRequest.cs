using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkyOne.AppServices.Interfaces.Repositories.Schedule.Requests
{
    /// <summary>
    /// Запрос на получение расписания(ий) из базы данных
    /// </summary>
    public class ScheduleRequest
    {
        /// <summary>
        /// ID расписания, которое требуется получить из базы данных
        /// </summary>
        public string? ScheduleId { get; set; }

        /// <summary>
        /// ID пользовательских данных, для которых требуется получить список расписаний из базы данных
        /// </summary>
        public string? UserDataId { get; set; }

        /// <summary>
        /// Включать ли шаблон в запрашиваемое расписание
        /// </summary>
        public bool IncludeTemplate { get; set; } = false;

        /// <summary>
        /// Включать ли в расписание список "датированных" смен
        /// </summary>

        public bool IncludeDatedShifts { get; set; } = false;

        /// <summary>
        /// Включать ли в расписание список "периодических" смен
        /// </summary>
        public bool IncludePeriodicShifst { get; set; } = false;
    }
}
