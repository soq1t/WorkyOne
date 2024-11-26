using WorkyOne.Domain.Interfaces.Common;
using WorkyOne.Domain.Requests.Common;

namespace WorkyOne.AppServices.Interfaces.Repositories.CRUD
{
    /// <summary>
    /// Интерфейс репозитория, который реализует выполнение всех CRUD операций в базе данных с сущностями типа <typeparamref name="TEntity"/>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ICrudRepository<TEntity, TEntityRequest, TPaginatedRequest>
        : ICreateRepository<TEntity>,
            IGetRepository<TEntity, TEntityRequest, TPaginatedRequest>,
            IUpdateRepository<TEntity>,
            IDeleteRepository<TEntity>,
            IRenewRepository<TEntity>
        where TEntity : class, IEntity
        where TEntityRequest : EntityRequest<TEntity>
        where TPaginatedRequest : PaginatedRequest<TEntity> { }
}
