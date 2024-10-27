using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.DTOs.Abstractions;
using WorkyOne.Contracts.DTOs.Schedule.Shifts;

namespace WorkyOne.Contracts.DTOs.Schedule.Common
{
    /// <summary>
    /// DTO для TemplateEntity
    /// </summary>
    public class TemplateDto : DtoBase
    {
        /// <summary>
        /// Список рабочих смен, используемых в шаблоне
        /// </summary>
        [Required]
        public List<TemplatedShiftDto> Shifts { get; set; } = new List<TemplatedShiftDto>();

        /// <summary>
        /// Последовательность смен в шаблоне
        /// </summary>
        [Required]
        public List<ShiftSequenceDto> Sequences { get; set; } = new List<ShiftSequenceDto>();

        /// <summary>
        /// Дата, с которой начинается отсчёт повторений шаблона
        /// </summary>
        [Required]
        public DateOnly? StartDate { get; set; }
    }
}
