using System.Linq.Expressions;
using WorkyOne.Contracts.Repositories.Common;
using WorkyOne.Domain.Interfaces.Common;

namespace WorkyOne.AppServices.Interfaces.Repositories.CRUD
{
    /// <summary>
    /// Интерфейс репозитория, который может удалять в базе данных сущности типа <typeparamref name="TEntity"/> согласно определённому условию
    /// </summary>
    /// <typeparam name="TEntity">Тип сущностей</typeparam>
    public interface IDeleteByConditionRepository<TEntity> : ISaveChangesRepository
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Удаляет из базы данных множество сущностей <typeparamref name="TEntity"/>, удовлетворяющих заданному условию
        /// </summary>
        /// <param name="predicate">Условие удаления сущностей</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<RepositoryResult> DeleteByConditionAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellation = default
        );
    }
}
