using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Interfaces.Common;

namespace WorkyOne.Domain.Entities.Schedule.Shifts
{
    /// <summary>
    /// Сущность, описывающая смену, которая длится определённое количество дней
    /// </summary>
    public sealed class PeriodicShiftEntity : ShiftEntity, IUpdatable<PeriodicShiftEntity>
    {
        /// <summary>
        /// Дата начала действия смены
        /// </summary>
        [Required]
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// Дата окончания действия смены
        /// </summary>
        [Required]
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

        public void UpdateFields(PeriodicShiftEntity entity)
        {
            StartDate = entity.StartDate;
            EndDate = entity.EndDate;
            ScheduleId = entity.ScheduleId;

            base.UpdateFields(entity);
        }
    }
}
