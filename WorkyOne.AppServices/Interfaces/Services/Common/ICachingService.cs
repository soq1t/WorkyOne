namespace WorkyOne.AppServices.Interfaces.Services.Common
{
    /// <summary>
    /// Интерфейс сервиса, работающего с кешем
    /// </summary>
    public interface ICachingService
    {
        /// <summary>
        /// Возвращает значение из кеша. При остутствии значения, выполняет <paramref name="function"/>, возвращает результат выполнения и сохраняет его в кеш
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого объекта</typeparam>
        /// <param name="key">Ключ для поиска в кеше</param>
        /// <param name="duration">Период, на который сохраняется результат выполнения <paramref name="function"/></param>
        /// <param name="function">Выполняемая функция по получению данных</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<T?> GetAsync<T>(
            string key,
            Func<Task<T?>> function,
            TimeSpan duration,
            CancellationToken cancellation = default
        )
            where T : class;

        /// <summary>
        /// Сохраняет объект в кеше
        /// </summary>
        /// <typeparam name="T">Тип сохраняемого объекта</typeparam>
        /// <param name="key">Ключ для сохранения в кеше</param>
        /// <param name="value">Сохраняемый объект</param>
        /// <param name="duration">Период, на который сохраняется объект</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task SaveAsync<T>(
            string key,
            T value,
            TimeSpan duration,
            CancellationToken cancellation = default
        );
    }
}
