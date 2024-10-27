using System.ComponentModel.DataAnnotations;

namespace WorkyOne.Contracts.Services.GetRequests.Common
{
    /// <summary>
    /// Пагинированный запрос на получение данных из репозитория
    /// </summary>
    public class PaginatedRequest
    {
        /// <summary>
        /// Номер страницы
        /// </summary>
        [Required(ErrorMessage = "Введите номер страницы")]
        [Range(1, int.MaxValue, ErrorMessage = "Введите корректный номер страницы (более 1)")]
        public int PageIndex { get; set; }

        /// <summary>
        /// Количество элементов на странице
        /// </summary>
        [Required(ErrorMessage = "Введите количество элементов на странице")]
        [Range(
            1,
            100,
            ErrorMessage = "Количество элементов на странице должно составлять от 1 до 100"
        )]
        public int Amount { get; set; }
    }
}
