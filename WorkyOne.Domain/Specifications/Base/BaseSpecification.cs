using System.Linq.Expressions;
using WorkyOne.Domain.Interfaces.Specification;
using WorkyOne.Domain.Specifications.Common;

namespace WorkyOne.Domain.Specifications.Base
{
    public abstract class BaseSpecification<T> : ISpecification<T>
        where T : class
    {
        public ISpecification<T> And(ISpecification<T> other)
        {
            return new AndSpecification<T>(this, other);
        }

        public ISpecification<T> Or(ISpecification<T> other)
        {
            return new OrSpecification<T>(this, other);
        }

        public abstract Expression<Func<T, bool>> ToExpression();
    }
}
