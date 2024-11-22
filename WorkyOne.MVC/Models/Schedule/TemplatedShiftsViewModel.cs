using WorkyOne.Contracts.DTOs.Schedule.Shifts.Basic;
using WorkyOne.Contracts.DTOs.Schedule.Shifts.Special;

namespace WorkyOne.MVC.Models.Schedule
{
    /// <summary>
    /// Модель для "шабонных" смен
    /// </summary>
    public class TemplatedShiftsViewModel
    {
        /// <summary>
        /// Список шаблонных смен
        /// </summary>
        public List<TemplatedShiftDto> Shifts { get; set; } = [];

        /// <summary>
        /// Список "персональных" смен
        /// </summary>
        public List<PersonalShiftDto> PersonalShifts { get; set; } = [];

        /// <summary>
        /// Список "общих" смен
        /// </summary>
        public List<SharedShiftDto> SharedShifts { get; set; } = [];
    }
}
