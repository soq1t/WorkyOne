using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Domain.Abstractions;
using WorkyOne.Domain.Entities.Schedule.Shifts;

namespace WorkyOne.Domain.Entities.Schedule
{
    /// <summary>
    /// Сущность, описывающая расписание пользователя
    /// </summary>
    public sealed class ScheduleEntity : EntityBase
    {
        /// <summary>
        /// ID данных пользователя
        /// </summary>
        [Required]
        [ForeignKey(nameof(UserData))]
        public string UserDataId { get; set; }

        /// <summary>
        /// Данные пользователя, к которому относится расписание
        /// </summary>
        [Required]
        public UserDataEntity UserData { get; set; }

        /// <summary>
        /// ID шаблона
        /// </summary>
        [ForeignKey(nameof(Template))]
        public string? TemplateId { get; set; }

        /// <summary>
        /// Шаблон, который используется в текущем расписании
        /// </summary>
        public TemplateEntity? Template { get; set; }

        /// <summary>
        /// Название расписания
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Список смен, выставляемых на конкретную дату
        /// </summary>
        public List<DatedShiftEntity> DatedShifts { get; set; } = new List<DatedShiftEntity>();

        /// <summary>
        /// Список смен, установленных на определённый период дней
        /// </summary>
        public List<PeriodicShiftEntity> PeriodicShifts { get; set; } =
            new List<PeriodicShiftEntity>();
    }
}
