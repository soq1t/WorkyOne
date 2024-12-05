namespace WorkyOne.Contracts.Services.GetRequests.Users
{
    /// <summary>
    /// Фильтр на получение пользователей из базы данных
    /// </summary>
    public class UserFilter
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Роль пользователя
        /// </summary>
        public string? RoleName { get; set; }
    }
}
