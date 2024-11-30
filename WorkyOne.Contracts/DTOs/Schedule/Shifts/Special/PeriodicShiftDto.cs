using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.Attributes.Validation;
using WorkyOne.Contracts.DTOs.Abstractions;
using WorkyOne.Contracts.Enums.Attributes;

namespace WorkyOne.Contracts.DTOs.Schedule.Shifts.Special
{
    /// <summary>
    /// DTO для PeriodicShiftEntity
    /// </summary>
    public class PeriodicShiftDto : ShiftReferenceDto
    {
        /// <summary>
        /// Дата начала действия смены
        /// </summary>
        [Required(ErrorMessage = "Укажите дату начала")]
        [DateCompare(DateCompareMode.LessOrEquial, nameof(EndDate))]
        public DateOnly? StartDate { get; set; }

        /// <summary>
        /// Дата окончания действия смены
        /// </summary>
        [Required(ErrorMessage = "Укажите дату окончания")]
        [DateCompare(DateCompareMode.MoreOrEquial, nameof(StartDate))]
        public DateOnly? EndDate { get; set; }

        /// <summary>
        /// ID расписания
        /// </summary>
        [Required]
        public string ScheduleId { get; set; }
    }
}
