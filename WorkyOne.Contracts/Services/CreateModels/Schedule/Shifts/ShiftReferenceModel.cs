using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.DTOs.Abstractions;

namespace WorkyOne.Contracts.Services.CreateModels.Schedule.Shifts
{
    /// <summary>
    /// Модель, содержащая информацию о создаваемой смене
    /// </summary>
    /// <typeparam name="TShift">Тип создаваемой смены</typeparam>
    public class ShiftReferenceModel<TShift>
        where TShift : ShiftReferenceDto
    {
        /// <summary>
        /// Идентификатор "родителя" смены
        /// </summary>
        public string? ParentId { get; set; }

        /// <summary>
        /// DTO создаваемой смены
        /// </summary>
        [Required]
        public TShift Shift { get; set; }
    }
}
