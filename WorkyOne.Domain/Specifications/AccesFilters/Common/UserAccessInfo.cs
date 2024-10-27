namespace WorkyOne.Domain.Specifications.AccesFilters.Common
{
    /// <summary>
    /// Объект, описывающий уровень доступа пользователя к данным
    /// </summary>
    public sealed class UserAccessInfo
    {
        public UserAccessInfo(string? userDataId, string? userId, bool isAdmin)
        {
            UserDataId = userDataId;
            UserId = userId;
            IsAdmin = isAdmin;
        }

        /// <summary>
        /// Указывает, является ли пользователь администратором
        /// </summary>
        public bool IsAdmin { get; private set; }

        /// <summary>
        /// Идентификатор пользовательских данных, к связанным данным которых у пользователя есть доступ
        /// </summary>
        public string? UserDataId { get; private set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string? UserId { get; private set; }
    }
}
