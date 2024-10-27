using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.DTOs.Abstractions;

namespace WorkyOne.Contracts.DTOs.Schedule.Common
{
    /// <summary>
    /// DTO для ShiftSequenceEntity
    /// </summary>
    public class ShiftSequenceDto : DtoBase
    {
        /// <summary>
        /// ID смены, используемой в данной ShiftSequence
        /// </summary>
        [Required]
        public string ShiftId { get; set; }

        /// <summary>
        /// Указатель позиции смены в последовательности
        /// </summary>
        [Range(1, 31)]
        [Required]
        public int Position { get; set; }

        public string? Name { get; set; }

        public TimeOnly? Beginning { get; set; }
        public TimeOnly? Ending { get; set; }
    }
}
