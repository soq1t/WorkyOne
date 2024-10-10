using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Domain.Abstractions;

namespace WorkyOne.Domain.Entities.Schedule.Common
{
    /// <summary>
    /// Сущность, описывающая определённый день для определённого расписания
    /// </summary>
    public sealed class DailyInfoEntity : EntityBase
    {
        /// <summary>
        /// ID расписания
        /// </summary>
        [Required]
        [ForeignKey(nameof(Schedule))]
        public string ScheduleId { get; set; }

        /// <summary>
        /// Расписание, которое описывает сущность
        /// </summary>
        [Required]
        public ScheduleEntity Schedule { get; set; }

        /// <summary>
        /// Указывает, является ли день рабочим
        /// </summary>
        [Required]
        public bool IsBusyDay { get; set; }

        public DateOnly Date { get; set; }

        /// <summary>
        /// Время начала рабочей смены
        /// </summary>
        public TimeOnly? Beginning { get; set; }

        /// <summary>
        /// Время окончания рабочей смены
        /// </summary>
        public TimeOnly? Ending { get; set; }

        /// <summary>
        /// Продолжительность рабочей смены
        /// </summary>
        public TimeSpan? ShiftProlongation { get; set; }
    }
}
