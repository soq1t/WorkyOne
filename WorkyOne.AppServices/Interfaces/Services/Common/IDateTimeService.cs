namespace WorkyOne.AppServices.Interfaces.Services
{
    /// <summary>
    /// Интерфейс сервиса по работе с датой и временем
    /// </summary>
    public interface IDateTimeService
    {
        /// <summary>
        /// Возвращает текущую дату и время
        /// </summary>
        /// <returns></returns>
        public DateTime GetNow();

        /// <summary>
        /// Возвращает текущую дату и время по UTC
        /// </summary>
        /// <returns></returns>
        public DateTime GetUtcNow();
    }
}
