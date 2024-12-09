using WorkyOne.Contracts.DTOs.Common;
using WorkyOne.Contracts.Services.GetRequests.Users;
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

        /// <summary>
        /// Фильтр пользователей
        /// </summary>
        public UserFilter Filter { get; set; } = new UserFilter();
    }
}
