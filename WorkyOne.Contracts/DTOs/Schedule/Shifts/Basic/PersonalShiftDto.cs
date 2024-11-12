using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.DTOs.Abstractions;

namespace WorkyOne.Contracts.DTOs.Schedule.Shifts.Basic
{
    /// <summary>
    /// DTO для смены, создаваемой персонально в расписании
    /// </summary>
    public class PersonalShiftDto : ShiftDtoBase
    {
        /// <summary>
        /// Идентификатор расписания, к которому относится смена
        /// </summary>
        [Required]
        public string ScheduleId { get; set; }
    }
}
