using WorkyOne.Domain.Interfaces.Common;
using WorkyOne.Domain.Requests.Common;

namespace WorkyOne.AppServices.Interfaces.Repositories.CRUD
{
    /// <summary>
    /// Интерфейс репозитория, который может получить из базы данных сущности типа <typeparamref name="TEntity"/>
    /// </summary>
    /// <typeparam name="TEntity">Тип сущностей</typeparam>
    public interface IGetRepository<TEntity, TEntityRequest, TPaginatedRequest>
        where TEntity : class, IEntity
        where TEntityRequest : EntityRequest<TEntity>
        where TPaginatedRequest : PaginatedRequest<TEntity>
    {
        /// <summary>
        /// Возвращает из базы сущность типа <typeparamref name="TEntity"/> согласно запросу <typeparamref name="TSingleRequest"/>
        /// </summary>
        /// <param name="request">Запрос на получение сущности</param>
        /// <param name="cancellation">Токен отмены задания</param>
        public Task<TEntity?> GetAsync(
            TEntityRequest request,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Возвращает из базы данных множество сущностей <typeparamref name="TEntity"/> согласно запросу <typeparamref name="TPaginatedRequest"/>
        /// </summary>
        /// <param name="request">Запрос на получение сущностей</param>
        /// <param name="cancellation">Токен отмены задания</param>
        public Task<List<TEntity>> GetManyAsync(
            TPaginatedRequest request,
            CancellationToken cancellation = default
        );
    }
}
