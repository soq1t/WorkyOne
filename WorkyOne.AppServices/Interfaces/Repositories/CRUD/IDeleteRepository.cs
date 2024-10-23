using WorkyOne.Contracts.Repositories.Common;
using WorkyOne.Domain.Interfaces.Common;

namespace WorkyOne.AppServices.Interfaces.Repositories.CRUD
{
    /// <summary>
    /// Интерфейс репозитория, который может удалять в базе данных сущности типа <typeparamref name="TEntity"/>
    /// </summary>
    /// <typeparam name="TEntity">Тип сущностей</typeparam>
    public interface IDeleteRepository<TEntity> : ISaveChangesRepository
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Удаляет из базы данных сущность типа <typeparamref name="TEntity"/>
        /// </summary>
        /// <typeparam name="TEntity">Тип удаляемой сущности</typeparam>
        /// <param name="entityId">Идентификатор удаляемой сущности</param>
        /// <param name="cancellation">Токен отмены задания</param>
        public Task<RepositoryResult> DeleteAsync(
            string entityId,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Удаляет из базы данных множество сущностей типа <typeparamref name="TEntity"/>
        /// </summary>
        /// <typeparam name="TEntity">Тип удаляемых сущностей</typeparam>
        /// <param name="entitiesIds">Список идентификаторов удаляемых сущностей</param>
        /// <param name="cancellation">Токен отмены задания</param>
        public Task<RepositoryResult> DeleteManyAsync(
            IEnumerable<string> entitiesIds,
            CancellationToken cancellation = default
        );
    }
}
