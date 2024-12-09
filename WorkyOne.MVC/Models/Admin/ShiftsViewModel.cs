using WorkyOne.Contracts.DTOs.Schedule.Shifts.Basic;

namespace WorkyOne.MVC.Models.Admin
{
    /// <summary>
    /// Модель с данными для управления сменами приложения
    /// </summary>
    public class ShiftsViewModel
    {
        /// <summary>
        /// Список смен приложения
        /// </summary>
        public List<SharedShiftDto> Shifts { get; set; } = [];
    }
}
