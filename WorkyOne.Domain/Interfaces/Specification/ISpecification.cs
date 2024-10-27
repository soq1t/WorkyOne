using System.Linq.Expressions;

namespace WorkyOne.Domain.Interfaces.Specification
{
    /// <summary>
    /// Интерфейс спецификации
    /// </summary>
    /// <typeparam name="T">Тип объекта, проверяемого спецификацией</typeparam>
    public interface ISpecification<T>
        where T : class
    {
        /// <summary>
        /// Возвращает выражение спецификации
        /// </summary>
        public Expression<Func<T, bool>> ToExpression();

        /// <summary>
        /// Возвращает спецификацию, имеющую отношение "И" к текущей спецификации
        /// </summary>
        /// <param name="other">Иная спецификация</param>
        public ISpecification<T> And(ISpecification<T> other);

        /// <summary>
        /// Возвращает спецификацию, имеющую отношение "ИЛИ" к текущей спецификации
        /// </summary>
        /// <param name="other">Иная спецификация</param>
        public ISpecification<T> Or(ISpecification<T> other);
    }
}
