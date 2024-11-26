using WorkyOne.Contracts.Repositories.Result;
using WorkyOne.Domain.Interfaces.Common;

namespace WorkyOne.AppServices.Interfaces.Repositories.CRUD
{
    /// <summary>
    /// Интерфейс репозитория, который позволяет "обновлять" список <typeparamref name="TEntity"/>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRenewRepository<TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Обновляет список сущностей <paramref name="target"/> относительно списка <paramref name="source"/>
        /// </summary>
        /// <param name="target">Список "обновляемых" сущностей</param>
        /// <param name="source">Список, относительно которого происходит обновление</param>
        public RepositoryResult Renew(IEnumerable<TEntity> target, IEnumerable<TEntity> source);
    }
}
