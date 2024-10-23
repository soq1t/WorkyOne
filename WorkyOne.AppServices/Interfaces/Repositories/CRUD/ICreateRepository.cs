using WorkyOne.Contracts.Repositories.Common;
using WorkyOne.Domain.Interfaces.Common;

namespace WorkyOne.AppServices.Interfaces.Repositories.CRUD
{
    /// <summary>
    /// Интерфейс репозитория, который может создавать в базе данных сущности типа <typeparamref name="TEntity"/>
    /// </summary>
    /// <typeparam name="TEntity">Тип сущностей</typeparam>
    public interface ICreateRepository<TEntity> : ISaveChangesRepository
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Создаёт в базе данных сущность типа <typeparamref name="TEntity"/>
        /// </summary>
        /// <param name="entity">Создаваемая сущность</param>
        /// <param name="cancellation">Токен отмены задания</param>
        public Task<RepositoryResult> CreateAsync(
            TEntity entity,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Создаёт в базе данных множество сущностей типа <typeparamref name="TEntity"/>
        /// </summary>
        /// <param name="entities">Список создаваемых сущностей</param>
        /// <param name="cancellation">Токен отмены задания</param>
        public Task<RepositoryResult> CreateManyAsync(
            IEnumerable<TEntity> entities,
            CancellationToken cancellation = default
        );
    }
}
