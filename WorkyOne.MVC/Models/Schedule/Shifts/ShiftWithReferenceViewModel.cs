using WorkyOne.Contracts.DTOs.Abstractions;
using WorkyOne.Contracts.DTOs.Schedule.Shifts.Basic;

namespace WorkyOne.MVC.Models.Schedule.Shifts
{
    /// <summary>
    /// Модель для смены, у которой есть ссылка на список смен
    /// </summary>
    public abstract class ShiftWithReferenceViewModel
    {
        /// <summary>
        /// Список смен расписания
        /// </summary>
        public List<PersonalShiftDto> PersonalShifts { get; set; } = [];

        /// <summary>
        /// Список глобальных смен приложения
        /// </summary>
        public List<SharedShiftDto> SharedShifts { get; set; } = [];
    }
}
