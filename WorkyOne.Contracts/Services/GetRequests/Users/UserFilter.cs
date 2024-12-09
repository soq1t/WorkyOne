using System.ComponentModel;

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
        [DisplayName("Имя пользователя")]
        public string? UserName { get; set; }

        /// <summary>
        /// Роль пользователя
        /// </summary>
        [DisplayName("Роль пользователя")]
        public string? RoleName { get; set; }

        /// <summary>
        /// Показывать только активированных пользователей
        /// </summary>
        [DisplayName("Активирован")]
        public bool ShowActivated { get; set; } = true;

        /// <summary>
        /// Показывать только неактивированных пользователей
        /// </summary>
        [DisplayName("Неактивирован")]
        public bool ShowInactivated { get; set; } = true;
    }
}
