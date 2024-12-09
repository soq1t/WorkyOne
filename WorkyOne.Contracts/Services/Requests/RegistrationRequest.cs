namespace WorkyOne.Contracts.Services.Requests
{
    /// <summary>
    /// Запрос на регистрацию пользователя
    /// </summary>
    public class RegistrationRequest
    {
        /// <summary>
        /// Логин
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }
    }
}
