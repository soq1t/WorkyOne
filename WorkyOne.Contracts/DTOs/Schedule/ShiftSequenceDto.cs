using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Contracts.DTOs.Abstractions;

namespace WorkyOne.Contracts.DTOs.Schedule
{
    /// <summary>
    /// DTO для ShiftSequenceEntity
    /// </summary>
    public class ShiftSequenceDto : DtoBase
    {
        /// <summary>
        /// ID DTO
        /// </summary>
        public string Id { get; set; }

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
    }
}
