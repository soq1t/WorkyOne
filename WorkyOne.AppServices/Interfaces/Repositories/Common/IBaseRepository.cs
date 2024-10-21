using WorkyOne.Contracts.Repositories;
using WorkyOne.Domain.Entities.Abstractions.Common;
using WorkyOne.Domain.Interfaces;

namespace WorkyOne.AppServices.Interfaces.Repositories.Common
{
    /// <summary>
    /// Интерфейс базового репозитория
    /// </summary>
    public interface IBaseRepository
    {
        /// <summary>
        /// Возвращает из базы сущность типа <typeparamref name="TEntity"/> согласно её идентификатору
        /// </summary>
        /// <typeparam name="TEntity">Тип возвращаемой сущности</typeparam>
        /// <param name="entityId">Идентификатор запрашиваемой сущности</param>
        /// <param name="cancellation">Токен отмены задания</param>
        public Task<TEntity?> GetByIdAsync<TEntity>(
            string entityId,
            CancellationToken cancellation = default
        )
            where TEntity : EntityBase;

        /// <summary>
        /// Создаёт в базе данных сущность типа <typeparamref name="TEntity"/>
        /// </summary>
        /// <typeparam name="TEntity">Тип создаваемой сущности</typeparam>
        /// <param name="entity">Создаваемая сущность</param>
        /// <param name="cancellation">Токен отмены задания</param>
        public Task<RepositoryResult> CreateAsync<TEntity>(
            TEntity entity,
            CancellationToken cancellation = default
        )
            where TEntity : EntityBase;

        /// <summary>
        /// Создаёт в базе данных множество сущностей типа <typeparamref name="TEntity"/>
        /// </summary>
        /// <typeparam name="TEntity">Тип создаваемой сущности</typeparam>
        /// <param name="entities">Список создаваемых сущностей</param>
        /// <param name="cancellation">Токен отмены задания</param>
        public Task<RepositoryResult> CreateManyAsync<TEntity>(
            IEnumerable<TEntity> entities,
            CancellationToken cancellation = default
        )
            where TEntity : EntityBase;

        /// <summary>
        /// Удаляет из базы данных сущность типа <typeparamref name="TEntity"/>
        /// </summary>
        /// <typeparam name="TEntity">Тип удаляемой сущности</typeparam>
        /// <param name="entityId">Идентификатор удаляемой сущности</param>
        /// <param name="cancellation">Токен отмены задания</param>
        public Task<RepositoryResult> DeleteAsync<TEntity>(
            string entityId,
            CancellationToken cancellation = default
        )
            where TEntity : EntityBase;

        /// <summary>
        /// Удаляет из базы данных множество сущностей типа <typeparamref name="TEntity"/>
        /// </summary>
        /// <typeparam name="TEntity">Тип удаляемых сущностей</typeparam>
        /// <param name="entitiesIds">Список идентификаторов удаляемых сущностей</param>
        /// <param name="cancellation">Токен отмены задания</param>
        public Task<RepositoryResult> DeleteManyAsync<TEntity>(
            IEnumerable<string> entitiesIds,
            CancellationToken cancellation = default
        )
            where TEntity : EntityBase;
    }
}
