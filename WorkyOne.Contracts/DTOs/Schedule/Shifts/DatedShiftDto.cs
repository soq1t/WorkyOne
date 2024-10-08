using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Contracts.DTOs.Abstractions;

namespace WorkyOne.Contracts.DTOs.Schedule.Shifts
{
    /// <summary>
    /// DTO для DatedShiftEntity
    /// </summary>
    public class DatedShiftDto : ShiftDtoBase
    {
        /// <summary>
        /// Дата, на которую установлена смена
        /// </summary>
        [Required]
        public DateOnly Date { get; set; }
    }
}
