using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Domain.Abstractions;
using WorkyOne.Domain.Entities.Schedule.Shifts;
using WorkyOne.Domain.Interfaces.Common;

namespace WorkyOne.Domain.Entities.Schedule.Common
{
    /// <summary>
    /// Сущность, описывающая определённый день для определённого расписания
    /// </summary>
    public sealed class DailyInfoEntity : EntityBase, IUpdatable<DailyInfoEntity>
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
        public string Name { get; set; }

        /// <summary>
        /// Код цвета, которым выделяется данный день на графике
        /// </summary>
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
        public bool IsBusyDay { get; set; }

        /// <summary>
        /// Дата, которую описывает сущность
        /// </summary>
        [Required]
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

        public void UpdateFields(DailyInfoEntity entity)
        {
            base.UpdateFields(entity);

            ScheduleId = entity.ScheduleId;
            ColorCode = entity.ColorCode;
            IsBusyDay = entity.IsBusyDay;
            Date = entity.Date;
            Beginning = entity.Beginning;
            Ending = entity.Ending;
            ShiftProlongation = entity.ShiftProlongation;
        }

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
