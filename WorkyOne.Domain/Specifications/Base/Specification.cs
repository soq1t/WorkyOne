using System.Linq.Expressions;
using WorkyOne.Domain.Interfaces.Specification;

namespace WorkyOne.Domain.Specifications.Base
{
    /// <summary>
    /// Спецификация, позволяющая делать выборку из объектов по определённому условию
    /// </summary>
    /// <typeparam name="T">Тип объекта, к которому применяется спецификация</typeparam>
    public sealed class Specification<T> : BaseSpecification<T>, ISpecification<T>
        where T : class
    {
        /// <summary>
        /// Выражение, по которому производится выборка
        /// </summary>
        private readonly Expression<Func<T, bool>> _expression;

        public Specification(Expression<Func<T, bool>> expression)
        {
            _expression = expression;
        }

        public override Expression<Func<T, bool>> ToExpression() => _expression;
    }
}
