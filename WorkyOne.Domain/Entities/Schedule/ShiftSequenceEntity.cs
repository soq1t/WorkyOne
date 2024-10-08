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
    /// Сущность, описывающая последовательность TempladedShift в Template
    /// </summary>
    public sealed class ShiftSequenceEntity : EntityBase
    {
        /// <summary>
        /// ID шаблона
        /// </summary>
        [Required]
        [ForeignKey(nameof(Template))]
        public string TemplateId { get; set; }

        /// <summary>
        /// Шаблон, в которой находится последовательность, описываемая сущностью
        /// </summary>
        [Required]
        public TemplateEntity Template { get; set; }

        /// <summary>
        /// ID смены
        /// </summary>
        [Required]
        [ForeignKey(nameof(Shift))]
        public string ShiftId { get; set; }

        /// <summary>
        /// Смена, описываемая сущностью
        /// </summary>
        [Required]
        public TemplatedShiftEntity Shift { get; set; }

        /// <summary>
        /// Указатель позиции смены в последовательности
        /// </summary>
        [Range(1, 31)]
        [Required]
        public int Position { get; set; }
    }
}
