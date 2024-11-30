namespace WorkyOne.AppServices.Interfaces.Repositories.Context
{
    /// <summary>
    /// Интерфейс сервиса по взаимодействию с контекстом
    /// </summary>
    /// <typeparam name="T">Тип контекста</typeparam>
    public interface IContextService : IDisposable
    {
        /// <summary>
        /// Создаёт транзакцию
        /// </summary>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task CreateTransactionAsync(CancellationToken cancellation = default);

        /// <summary>
        /// Применяет изменения в транзакции
        /// </summary>
        /// <param name="cancellation">Токен отмены задачи</param>

        public Task CommitTransactionAsync(CancellationToken cancellation = default);

        /// <summary>
        /// Откатывает изменения в транзакции
        /// </summary>
        /// <param name="cancellation">Токен отмены задачи</param>

        public Task RollbackTransactionAsync(CancellationToken cancellation = default);
    }
}
