using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.DTOs.Abstractions;

namespace WorkyOne.MVC.Models.Schedule.Shifts
{
    /// <summary>
    /// Модель для смены
    /// </summary>
    public class ShiftViewModel
    {
        /// <summary>
        /// Смена
        /// </summary>
        [Required]
        public ShiftDtoBase Shift { get; set; }
    }
}
