using WorkyOne.Contracts.DTOs.Abstractions;

namespace WorkyOne.Contracts.Services.CreateModels.Schedule.Shifts
{
    /// <summary>
    /// Модель, содержащая информацию о создаваемой смене
    /// </summary>
    /// <typeparam name="TShift">Тип создаваемой смены</typeparam>
    public class ShiftModel<TShift>
        where TShift : ShiftDtoBase
    {
        /// <summary>
        /// Идентификатор "родителя" смены
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// DTO создаваемой смены
        /// </summary>
        public TShift Shift { get; set; }
    }
}
