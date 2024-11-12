using WorkyOne.Domain.Entities.Abstractions.Shifts;
using WorkyOne.Domain.Entities.Schedule.Common;

namespace WorkyOne.Domain.Entities.Schedule.Shifts.Basic
{
    /// <summary>
    /// Класс сущности, описывающей смену, которая может использоваться во всех расписаниях
    /// </summary>
    public class SharedShiftEntity : ShiftEntity
    {
        /// <summary>
        /// Расписания, в которых используется смена
        /// </summary>
        public List<ScheduleEntity> Schedules { get; set; } = [];
    }
}
