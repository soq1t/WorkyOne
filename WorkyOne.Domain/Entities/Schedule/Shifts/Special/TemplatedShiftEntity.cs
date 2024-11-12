using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkyOne.Domain.Attributes.Updating;
using WorkyOne.Domain.Entities.Abstractions.Shifts;
using WorkyOne.Domain.Entities.Schedule.Common;

namespace WorkyOne.Domain.Entities.Schedule.Shifts.Special
{
    /// <summary>
    /// Сущность смены, используемой в <see cref="TemplateEntity"/>
    /// </summary>
    public sealed class TemplatedShiftEntity : ShiftReferenceEntity
    {
        /// <summary>
        /// ID шаблона
        /// </summary>
        [Required]
        [ForeignKey(nameof(Template))]
        public string TemplateId { get; set; }

        /// <summary>
        /// Шаблон, к которому относится данная смена
        /// </summary>
        [Required]
        public TemplateEntity Template { get; set; }

        /// <summary>
        /// Указатель позиции смены в последовательности
        /// </summary>
        [Range(1, 31)]
        [Required]
        [AutoUpdated]
        public int Position { get; set; }
    }
}
