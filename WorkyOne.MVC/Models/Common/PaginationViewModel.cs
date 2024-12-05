namespace WorkyOne.MVC.Models.Common
{
    /// <summary>
    /// Вью модель для представления пагинации
    /// </summary>
    public class PaginationViewModel
    {
        /// <summary>
        /// Номер текущей страницы
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Общее количество страниц
        /// </summary>
        public int PagesAmount { get; set; }
    }
}
