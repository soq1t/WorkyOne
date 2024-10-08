using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Interfaces.Common;

namespace WorkyOne.Domain.Entities.Schedule.Shifts
{
    /// <summary>
    /// Сущность смены, используемой в <see cref="TemplateEntity"/>
    /// </summary>
    public sealed class TemplatedShiftEntity : ShiftEntity, IUpdatable<TemplatedShiftEntity>
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
        /// Список последовательностей, которые включают в себя данную схему
        /// </summary>
        [Required]
        public List<ShiftSequenceEntity> Sequences { get; set; } = new List<ShiftSequenceEntity>();

        public void UpdateFields(TemplatedShiftEntity entity)
        {
            TemplateId = entity.TemplateId;
            base.UpdateFields(entity);
        }
    }
}
