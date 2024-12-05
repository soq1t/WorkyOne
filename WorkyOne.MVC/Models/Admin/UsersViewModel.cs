using System.ComponentModel.DataAnnotations;
using WorkyOne.Contracts.DTOs.Common;
using WorkyOne.MVC.Models.Common;

namespace WorkyOne.MVC.Models.Admin
{
    /// <summary>
    /// Вью модель для страницы управления пользователями
    /// </summary>
    public class UsersViewModel
    {
        /// <summary>
        /// Список пользователей
        /// </summary>
        public List<UserInfoDto> Users { get; set; } = [];

        /// <summary>
        /// Модель данных пагинации
        /// </summary>
        public PaginationViewModel Pagination { get; set; } = new PaginationViewModel();
    }
}
