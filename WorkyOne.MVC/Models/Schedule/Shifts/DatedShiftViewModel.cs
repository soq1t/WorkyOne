using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.DTOs.Schedule.Shifts.Special;

namespace WorkyOne.MVC.Models.Schedule.Shifts
{
    /// <summary>
    /// Модель датированной смены
    /// </summary>
    public class DatedShiftViewModel : ShiftWithReferenceViewModel
    {
        /// <summary>
        /// Датированная смена
        /// </summary>
        [Required]
        public DatedShiftDto Shift { get; set; }
    }
}
