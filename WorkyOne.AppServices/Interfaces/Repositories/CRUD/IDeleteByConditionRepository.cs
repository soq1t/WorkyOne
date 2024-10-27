using WorkyOne.Contracts.Repositories.Result;
using WorkyOne.Domain.Interfaces.Common;
using WorkyOne.Domain.Interfaces.Specification;

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
        /// <param name="specification">Условие удаления сущностей</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<RepositoryResult> DeleteByConditionAsync(
            ISpecification<TEntity> specification,
            CancellationToken cancellation = default
        );
    }
}
