using WorkyOne.Contracts.DTOs.Abstractions;

namespace WorkyOne.Contracts.DTOs
{
    /// <summary>
    /// DTO данных о пользователе
    /// </summary>
    public class UserInfoDto : DtoBase
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Пользовательский юзернейм
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string FirstName { get; set; }
    }
}
