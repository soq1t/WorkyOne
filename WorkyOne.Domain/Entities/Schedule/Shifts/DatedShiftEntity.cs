﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkyOne.Domain.Entities.Schedule.Shifts
{
    /// <summary>
    /// Сущность, описывающая смену, которая выставляется на определённую дату
    /// </summary>
    public sealed class DatedShiftEntity : ShiftEntity
    {
        /// <summary>
        /// Дата, на которую установлена смена
        /// </summary>
        [Required]
        public DateOnly Date { get; set; }

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