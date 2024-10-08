using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Contracts.DTOs.Abstractions;

namespace WorkyOne.Contracts.DTOs.Schedule
{
    /// <summary>
    /// DTO для PeriodicShiftEntity
    /// </summary>
    public class PeriodicShiftDto : ShiftDtoBase
    {
        /// <summary>
        /// Дата начала действия смены
        /// </summary>
        [Required]
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// Дата окончания действия смены
        /// </summary>
        [Required]
        public DateOnly EndDate { get; set; }
    }
}
