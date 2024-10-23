using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Shifts;

namespace WorkyOne.Domain.Interfaces.Requests.Schedule
{
    /// <summary>
    /// Интерфейс запроса на получение <see cref="ScheduleEntity"/> из базы данных
    /// </summary>
    public interface IScheduleRequest
    {
        /// <summary>
        /// Включать ли информацию о <see cref="TemplateEntity"/> к возвращаемым сущностям
        /// </summary>
        public bool IncludeTemplate { get; }

        /// <summary>
        /// Включать ли информацию о <see cref="DatedShiftEntity"/>, <see cref="PeriodicShiftEntity"/> к возвращаемым сущностям
        /// </summary>
        public bool IncludeShifts { get; }
    }
}
