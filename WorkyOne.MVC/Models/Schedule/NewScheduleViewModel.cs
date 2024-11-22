using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WorkyOne.MVC.Models.Schedule
{
    /// <summary>
    /// Модель для создания нового расписания
    /// </summary>
    public class NewScheduleViewModel
    {
        /// <summary>
        /// Название расписания
        /// </summary>
        [Required(ErrorMessage = "Заполните название расписания")]
        [MaxLength(100)]
        [DisplayName("Название расписания")]
        public string ScheduleName { get; set; }

        /// <summary>
        /// Идентификатор данных пользователя
        /// </summary>
        [Required]
        public string UserDataId { get; set; }
    }
}
