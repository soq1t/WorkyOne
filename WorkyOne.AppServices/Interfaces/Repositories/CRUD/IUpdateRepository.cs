using WorkyOne.Contracts.Repositories.Result;
using WorkyOne.Domain.Interfaces.Common;

namespace WorkyOne.AppServices.Interfaces.Repositories.CRUD
{
    /// <summary>
    /// Интерфейс репозитория, который может обновлять в базе данных сущности типа <typeparamref name="TEntity"/>
    /// </summary>
    /// <typeparam name="TEntity">Тип сущностей</typeparam>
    public interface IUpdateRepository<TEntity> : ISaveChangesRepository
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Производит обновление сущности <paramref name="entity"/> в базе данных
        /// </summary>
        /// <param name="entity">Обновляемая сущность</param>
        public RepositoryResult Update(TEntity entity);

        /// <summary>
        /// Производит обновление множества сущностей <paramref name="entity"/> в базе данных
        /// </summary>
        /// <param name="entities">Список обновляемых сущностей</param>
        public RepositoryResult UpdateMany(IEnumerable<TEntity> entities);
    }
}
