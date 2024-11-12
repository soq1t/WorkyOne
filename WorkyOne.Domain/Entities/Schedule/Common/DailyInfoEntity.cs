using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkyOne.Domain.Attributes.Updating;
using WorkyOne.Domain.Entities.Abstractions.Common;
using WorkyOne.Domain.Entities.Abstractions.Shifts;

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
        public string? ColorCode { get; set; }

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
    }
}
