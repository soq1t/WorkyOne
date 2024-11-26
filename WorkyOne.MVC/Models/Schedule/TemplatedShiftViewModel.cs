using WorkyOne.Contracts.DTOs.Abstractions;
using WorkyOne.Contracts.DTOs.Schedule.Shifts.Special;

namespace WorkyOne.MVC.Models.Schedule
{
    /// <summary>
    /// Модель для создания шаблонной смены
    /// </summary>
    public class TemplatedShiftViewModel
    {
        public TemplatedShiftDto TemplatedShift { get; set; }

        public ShiftDtoBase ReferenceShift { get; set; }

        public int Index { get; set; }
    }
}
