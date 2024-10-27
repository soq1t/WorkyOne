using WorkyOne.Contracts.Repositories.Result;
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
        /// <param name="entity">Удаляемая сущность</param>
        public RepositoryResult Delete(TEntity entity);

        /// <summary>
        /// Удаляет из базы данных множество сущностей типа <typeparamref name="TEntity"/>
        /// </summary>
        /// <param name="entities">Удаляемые сущности</param>
        public RepositoryResult DeleteMany(IEnumerable<TEntity> entities);
    }
}
