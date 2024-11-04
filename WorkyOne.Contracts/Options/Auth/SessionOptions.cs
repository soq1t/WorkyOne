namespace WorkyOne.Contracts.Options.Auth
{
    /// <summary>
    /// Конфигурация для сессионного токена
    /// </summary>
    public class SessionOptions
    {
        /// <summary>
        /// Название сессионного токена при сохранении в кукис
        /// </summary>
        public string CookiesName { get; set; }

        /// <summary>
        /// Время действия токена (в днях)
        /// </summary>

        public int ExpirationDays { get; set; }
    }
}
