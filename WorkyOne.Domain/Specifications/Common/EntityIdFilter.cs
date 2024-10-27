using System.Linq.Expressions;
using WorkyOne.Domain.Interfaces.Common;
using WorkyOne.Domain.Interfaces.Specification;
using WorkyOne.Domain.Specifications.Base;

namespace WorkyOne.Domain.Specifications.Common
{
    /// <summary>
    /// Спецификация, выполняющая сортировку по идентификатору <typeparamref name="TEntity"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class EntityIdFilter<TEntity>
        : BaseSpecification<TEntity>,
            ISpecification<TEntity>
        where TEntity : class, IEntity
    {
        private readonly string _id;

        public EntityIdFilter(string id)
        {
            _id = id;
        }

        public override Expression<Func<TEntity, bool>> ToExpression() => x => x.Id == _id;
    }
}
