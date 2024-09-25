using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Domain.Abstractions;

namespace WorkyOne.Domain.Entities.Schedule
{
    /// <summary>
    /// Сущность, описывающая смену, выставляемую только на один день
    /// </summary>
    public class SingleDayShiftEntity : EntityBase
    {
        /// <summary>
        /// Идентификатор смены, описываемой данной сущностью
        /// </summary>
        [Required]
        [ForeignKey(nameof(Shift))]
        public string ShiftId { get; set; }

        /// <summary>
        /// Смена, которая описывается данной сущностью
        /// </summary>
        [Required]
        public ShiftEntity Shift { get; set; }

        /// <summary>
        /// Идентификатор шаблона, к которому относится данная сущность
        /// </summary>
        [Required]
        [ForeignKey(nameof(Template))]
        public string TemplateId { get; set; }

        /// <summary>
        /// Шаблон, к которому относится данная сущность
        /// </summary>
        [Required]
        public TemplateEntity Template { get; set; }

        /// <summary>
        /// Дата, на которую устанавливается данная смена
        /// </summary>
        [Required]
        public DateOnly ShiftDate { get; set; }
    }
}
