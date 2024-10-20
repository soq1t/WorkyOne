using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.DTOs.Schedule.Shifts;

namespace WorkyOne.MVC.ViewModels.Api.Schedule.Shifts
{
    /// <summary>
    /// Вьюмодель для <see cref="DatedShiftDto"/>
    /// </summary>
    public sealed class DatedShiftViewModel
    {
        /// <summary>
        /// Идентификатор расписания, к оторому относится <see cref="DatedShiftDto"/>
        /// </summary>
        [Required]
        public string ScheduleId { get; set; }

        /// <summary>
        /// DTO "датированной смены"
        /// </summary>
        [Required]
        public DatedShiftDto DatedShift { get; set; }
    }
}
