using WorkyOne.Contracts.Services.GetRequests.Users;

namespace WorkyOne.Contracts.Services.Requests
{
    /// <summary>
    /// Запрос на получение списка пользователей
    /// </summary>
    public class UsersRequest
    {
        /// <summary>
        /// Фильтр пользователей
        /// </summary>
        public UserFilter Filter { get; set; }

        /// <summary>
        /// Страница списка пользователей
        /// </summary>
        public int Page { get; set; } = 1;
    }
}
