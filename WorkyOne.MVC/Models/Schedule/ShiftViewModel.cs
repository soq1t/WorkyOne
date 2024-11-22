using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.DTOs.Abstractions;
using WorkyOne.Contracts.Enums.Reposistories;

namespace WorkyOne.MVC.Models.Schedule
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

        /// <summary>
        /// Тип смены
        /// </summary>
        [Required]
        public ShiftType Type { get; set; }
    }
}
