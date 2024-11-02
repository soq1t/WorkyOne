namespace WorkyOne.Contracts.Configuration
{
    /// <summary>
    /// Конфигурация для JWT
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
    }
}
