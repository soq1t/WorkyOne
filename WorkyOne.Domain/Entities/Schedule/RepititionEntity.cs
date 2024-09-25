using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkyOne.Domain.Abstractions;

namespace WorkyOne.Domain.Entities.Schedule
{
    /// <summary>
    /// Сущность, описывающая количество повторений заданной смены в шаблоне
    /// </summary>
    public class RepititionEntity : EntityBase
    {
        /// <summary>
        /// Идентификатор смены
        /// </summary>
        [Required]
        [ForeignKey(nameof(Shift))]
        public string ShiftId { get; set; }

        /// <summary>
        /// Смена
        /// </summary>
        [Required]
        public ShiftEntity Shift { get; set; }

        /// <summary>
        /// Количество повторений смены, указанной в поле Shift
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int RepetitionAmount { get; set; }

        /// <summary>
        /// Порядковый номер повторения в шаблоне
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int Position { get; set; }
    }
}
