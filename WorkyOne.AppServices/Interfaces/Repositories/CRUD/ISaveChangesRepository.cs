namespace WorkyOne.AppServices.Interfaces.Repositories.CRUD
{
    /// <summary>
    /// Интерфейс репозитория, который может сохранять изменения, внесённые в базу данных
    /// </summary>
    public interface ISaveChangesRepository
    {
        /// <summary>
        /// Сохраняет внесённые в базу данных изменения
        /// </summary>
        /// <param name="cancellation">Токен отмены задания</param>
        /// <returns></returns>
        public Task SaveChangesAsync(CancellationToken cancellation = default);
    }
}
