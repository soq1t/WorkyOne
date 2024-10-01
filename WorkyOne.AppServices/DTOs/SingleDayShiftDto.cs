using WorkyOne.AppServices.DTOs.Abstractions;

namespace WorkyOne.AppServices.DTOs
{
    /// <summary>
    /// DTO "однодневной" смены
    /// </summary>
    public class SingleDayShiftDto : DtoBase
    {
        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Информация о смене, описываемая данной DTO
        /// </summary>
        public ShiftDto Shift { get; set; }

        /// <summary>
        /// Дата, на которую устанавливается данная смена
        /// </summary>
        public DateOnly ShiftDate { get; set; }
    }
}
