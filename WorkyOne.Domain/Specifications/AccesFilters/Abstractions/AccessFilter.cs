using WorkyOne.Domain.Interfaces.Common;
using WorkyOne.Domain.Interfaces.Specification;
using WorkyOne.Domain.Specifications.AccesFilters.Common;
using WorkyOne.Domain.Specifications.Base;

namespace WorkyOne.Domain.Specifications.AccesFilters.Abstractions
{
    /// <summary>
    /// Спецификация, фильтрующая доступ к <typeparamref name="TEntity"/>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class AccessFilter<TEntity>
        : BaseSpecification<TEntity>,
            ISpecification<TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Информация о уровне доступа пользователя к информации в базе данных
        /// </summary>
        protected UserAccessInfo _accessInfo;

        protected AccessFilter(UserAccessInfo accessInfo)
        {
            _accessInfo = accessInfo;
        }
    }
}
