using System.ComponentModel.DataAnnotations;

namespace WorkyOne.Contracts.Services.Requests
{
    /// <summary>
    /// Запрос, содержащий данные для входа в аккаунт пользователя
    /// </summary>
    public class LogInRequest
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Нужно ли создавать долгосрочную сессию для этого входа
        /// </summary>
        public bool CreateSession { get; set; } = false;
    }
}
