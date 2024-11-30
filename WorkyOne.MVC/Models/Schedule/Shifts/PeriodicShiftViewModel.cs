using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.DTOs.Schedule.Shifts.Special;

namespace WorkyOne.MVC.Models.Schedule.Shifts
{
    /// <summary>
    /// Модель периодической смены смены
    /// </summary>
    public class PeriodicShiftViewModel : ShiftWithReferenceViewModel
    {
        /// <summary>
        /// Периодическая смена
        /// </summary>
        [Required]
        public PeriodicShiftDto Shift { get; set; }
    }
}
