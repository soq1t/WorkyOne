using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkyOne.Domain.Attributes.Updating;
using WorkyOne.Domain.Entities.Abstractions.Common;
using WorkyOne.Domain.Entities.Schedule.Shifts;

namespace WorkyOne.Domain.Entities.Schedule.Common
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
        [AutoUpdated]
        public int Position { get; set; }
    }
}
