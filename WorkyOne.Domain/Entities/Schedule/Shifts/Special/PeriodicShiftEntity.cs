using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkyOne.Domain.Attributes.Updating;
using WorkyOne.Domain.Entities.Abstractions.Shifts;
using WorkyOne.Domain.Entities.Schedule.Common;

namespace WorkyOne.Domain.Entities.Schedule.Shifts.Special
{
    /// <summary>
    /// Сущность, описывающая смену, которая длится определённое количество дней
    /// </summary>
    public sealed class PeriodicShiftEntity : ShiftReferenceEntity
    {
        /// <summary>
        /// Дата начала действия смены
        /// </summary>
        [Required]
        [AutoUpdated]
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// Дата окончания действия смены
        /// </summary>
        [Required]
        [AutoUpdated]
        public DateOnly EndDate { get; set; }

        /// <summary>
        /// ID расписания
        /// </summary>
        [Required]
        [ForeignKey(nameof(Schedule))]
        public string ScheduleId { get; set; }

        /// <summary>
        /// Расписание, в котором используется данная смена
        /// </summary>
        [Required]
        public ScheduleEntity Schedule { get; set; }
    }
}
