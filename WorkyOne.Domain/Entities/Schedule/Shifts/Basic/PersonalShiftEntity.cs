using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkyOne.Domain.Entities.Abstractions.Shifts;
using WorkyOne.Domain.Entities.Schedule.Common;

namespace WorkyOne.Domain.Entities.Schedule.Shifts.Basic
{
    /// <summary>
    /// Класс, описывающий смену, создаваемую для определённого расписания
    /// </summary>
    public class PersonalShiftEntity : ShiftEntity
    {
        /// <summary>
        /// Идентификатор расписания, к которому относится смена
        /// </summary>
        [Required]
        [ForeignKey(nameof(Schedule))]
        public string ScheduleId { get; set; }

        /// <summary>
        /// Расписание, к которому относится смена
        /// </summary>
        [Required]
        public ScheduleEntity Schedule { get; set; }
    }
}
