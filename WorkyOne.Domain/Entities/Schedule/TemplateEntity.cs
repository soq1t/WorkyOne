using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WorkyOne.Domain.Abstractions;
using WorkyOne.Domain.Entities.Schedule.Shifts;

namespace WorkyOne.Domain.Entities.Schedule
{
    /// <summary>
    /// Сущность, описывающая определённую последовательность смен
    /// </summary>
    public sealed class TemplateEntity : EntityBase
    {
        /// <summary>
        /// ID расписания
        /// </summary>
        [Required]
        [ForeignKey(nameof(Schedule))]
        public string ScheduleId { get; set; }

        /// <summary>
        /// Расписание, к которому относится данный шаблон
        /// </summary>
        [Required]
        public ScheduleEntity Schedule { get; set; }

        /// <summary>
        /// Строка, описывающая последовательность смен в виде символов (напр. "ДНВВ")
        /// </summary>
        [MaxLength(31)]
        [Required]
        public string ShiftsQuery { get; set; } = string.Empty;

        /// <summary>
        /// Список рабочих смен, используемых в шаблоне
        /// </summary>
        [Required]
        public List<TemplatedShiftEntity> Shifts { get; set; } = new List<TemplatedShiftEntity>();

        /// <summary>
        /// Дата, с которой начинается отсчёт повторений шаблона
        /// </summary>
        [Required]
        public DateOnly StartDate { get; set; }
    }
}
