using System.ComponentModel.DataAnnotations;
using WorkyOne.MVC.Models.Schedule;

namespace WorkyOne.MVC.Models.Common
{
    /// <summary>
    /// Вью модель для главной страницы
    /// </summary>
    public class HomeViewModel
    {
        /// <summary>
        /// Вью модель календаря с графиком
        /// </summary>
        [Required]
        public CalendarViewModel CalendarViewModel { get; set; }
    }
}
