using WorkyOne.Domain.Interfaces.Common;
using WorkyOne.Domain.Specifications.AccesFilters.Abstractions;

namespace WorkyOne.AppServices.Interfaces.Stores
{
    /// <summary>
    /// Интерфейс хранилища фильтов доступа к данным
    /// </summary>
    public interface IAccessFiltersStore
    {
        /// <summary>
        /// Создаёт фильтры доступа к данным
        /// </summary>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task CreateFiltersAsync(CancellationToken cancellation = default);

        /// <summary>
        /// Возвращает фильтр доступа для <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Тип сущности</typeparam>
        public AccessFilter<T> GetFilter<T>()
            where T : class, IEntity;
    }
}
