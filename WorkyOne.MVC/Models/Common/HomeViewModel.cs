using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.Services.Common;

namespace WorkyOne.MVC.Models.Common
{
    /// <summary>
    /// Вью модель для главной страницы
    /// </summary>
    public class HomeViewModel
    {
        /// <summary>
        /// Год, отображаемый календарём на странице
        /// </summary>
        [Required]
        [Range(1900, 3000)]
        public int Year { get; set; }

        /// <summary>
        /// Месяц, отображаемый календарём на странице
        /// </summary>
        [Required]
        [Range(1, 12)]
        public int Month { get; set; }
    }
}
