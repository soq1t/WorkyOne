using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.DTOs.Abstractions;

namespace WorkyOne.Contracts.DTOs.Schedule.Shifts.Special
{
    /// <summary>
    /// DTO для TemplatedShiftEntity
    /// </summary>
    public sealed class TemplatedShiftDto : ShiftReferenceDto
    {
        /// <summary>
        /// Указатель позиции смены в последовательности
        /// </summary>
        [Range(1, 31)]
        [Required]
        public int Position { get; set; }
    }
}
