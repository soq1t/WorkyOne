using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.Attributes.Validation;
using WorkyOne.Contracts.DTOs.Abstractions;

namespace WorkyOne.Contracts.DTOs.Schedule.Shifts.Special
{
    /// <summary>
    /// DTO для DatedShiftEntity
    /// </summary>
    public sealed class DatedShiftDto : ShiftReferenceDto
    {
        /// <summary>
        /// Дата, на которую установлена смена
        /// </summary>
        [Required]
        [DateRequired]
        public DateOnly Date { get; set; }
    }
}
