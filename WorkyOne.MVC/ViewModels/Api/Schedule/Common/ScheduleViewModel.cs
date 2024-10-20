using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.DTOs.Schedule.Common;

namespace WorkyOne.MVC.ViewModels.Api.Schedule.Common
{
    /// <summary>
    /// Вьюмодель для расписания
    /// </summary>
    public sealed class ScheduleViewModel
    {
        /// <summary>
        /// Идентификатор пользовательских данных, к которым относится расписание
        /// </summary>
        [Required]
        public string UserDataId { get; set; }

        /// <summary>
        /// Название расписания
        /// </summary>
        [Required]
        public string ScheduleName { get; set; }
    }
}
