using System.Linq.Expressions;
using WorkyOne.Domain.Interfaces.Specification;

namespace WorkyOne.Domain.Specifications.Base
{
    public sealed class OrSpecification<T> : BaseSpecification<T>, ISpecification<T>
        where T : class
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;

        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var param = Expression.Parameter(typeof(T));

            var left = _left.ToExpression();
            var right = _right.ToExpression();

            var and = Expression.OrElse(
                Expression.Invoke(left, param),
                Expression.Invoke(right, param)
            );

            return Expression.Lambda<Func<T, bool>>(and, param);
        }
    }
}
