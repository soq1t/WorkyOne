using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.DTOs.Abstractions;
using WorkyOne.Contracts.DTOs.Schedule.Shifts.Special;

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
        /// Дата, с которой начинается отсчёт повторений шаблона
        /// </summary>
        [Required(ErrorMessage = "Укажите дату отсчёта шаблона")]
        public DateOnly? StartDate { get; set; }
    }
}
