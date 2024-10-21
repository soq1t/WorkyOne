using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Domain.Attributes.Updating;
using WorkyOne.Domain.Entities.Abstractions.Common;
using WorkyOne.Domain.Entities.Abstractions.Shifts;
using WorkyOne.Domain.Interfaces.Common;

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
        /// Название рабочего дня
        /// </summary>
        [Required]
        [AutoUpdated]
        public string Name { get; set; }

        /// <summary>
        /// Код цвета, которым выделяется данный день на графике
        /// </summary>
        [AutoUpdated]
        public string ColorCode { get; set; } = "#FFFFFF";

        /// <summary>
        /// Расписание, которое описывает сущность
        /// </summary>
        [Required]
        public ScheduleEntity Schedule { get; set; }

        /// <summary>
        /// Указывает, является ли день рабочим
        /// </summary>
        [Required]
        [AutoUpdated]
        public bool IsBusyDay { get; set; }

        /// <summary>
        /// Дата, которую описывает сущность
        /// </summary>
        [Required]
        [AutoUpdated]
        public DateOnly Date { get; set; }

        /// <summary>
        /// Время начала рабочей смены
        /// </summary>
        [AutoUpdated]
        public TimeOnly? Beginning { get; set; }

        /// <summary>
        /// Время окончания рабочей смены
        /// </summary>
        [AutoUpdated]
        public TimeOnly? Ending { get; set; }

        /// <summary>
        /// Продолжительность рабочей смены
        /// </summary>
        [AutoUpdated]
        public TimeSpan? ShiftProlongation { get; set; }

        public static DailyInfoEntity CreateFromShiftEntity(ShiftEntity shift, DateOnly date)
        {
            var info = new DailyInfoEntity();

            info.Name = shift.Name;
            info.ColorCode = shift.ColorCode ?? "#FFFFFF";
            info.IsBusyDay = shift.Beginning.HasValue && shift.Ending.HasValue;
            info.Date = date;

            if (info.IsBusyDay)
            {
                info.Beginning = shift.Beginning.Value;
                info.Ending = shift.Ending.Value;
                info.ShiftProlongation = shift.Duration();
            }

            return info;
        }
    }
}
