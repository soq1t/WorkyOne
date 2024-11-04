namespace WorkyOne.Contracts.Options.Auth
{
    /// <summary>
    /// Конфигурация для JWT токена
    /// </summary>
    public class JwtOptions
    {
        /// <summary>
        /// Издатель токена
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Потребитель токена
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Секретный ключ
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Название JWT токена при сохранении в кукис
        /// </summary>
        public string CookiesName { get; set; }

        /// <summary>
        /// Время действия JWT токена (в минутах)
        /// </summary>
        public int ExpirationMinutes { get; set; }
    }
}
