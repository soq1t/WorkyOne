using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.DTOs.Schedule.Shifts.Basic;

namespace WorkyOne.Contracts.Services.CreateModels.Schedule.Shifts
{
    /// <summary>
    /// Модель для создания "персональной" смены
    /// </summary>
    public class PersonalShiftModel
    {
        /// <summary>
        /// Идентификатор расписания, для которого создаётся смена
        /// </summary>
        public string? ScheduleId { get; set; }

        /// <summary>
        /// DTO создаваемой смены
        /// </summary>
        [Required]
        public PersonalShiftDto Shift { get; set; }
    }
}
