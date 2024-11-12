using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkyOne.Domain.Attributes.Updating;
using WorkyOne.Domain.Entities.Abstractions.Common;

namespace WorkyOne.Domain.Entities.Abstractions.Shifts
{
    /// <summary>
    /// Асбтракция сущности, имеющей ссылку на <see cref="ShiftEntity"/>
    /// </summary>
    public abstract class ShiftReferenceEntity : EntityBase
    {
        /// <summary>
        /// Идентификатор смены, на которую ссылается данная сущность
        /// </summary>
        [Required]
        [ForeignKey(nameof(Shift))]
        [AutoUpdated]
        public string ShiftId { get; set; }

        /// <summary>
        /// Смена, на которую ссылается данная сущность
        /// </summary>
        [Required]
        public ShiftEntity Shift { get; set; }
    }
}
