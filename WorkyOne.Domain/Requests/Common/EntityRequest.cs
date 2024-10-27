using WorkyOne.Domain.Interfaces.Common;
using WorkyOne.Domain.Interfaces.Specification;

namespace WorkyOne.Domain.Requests.Common
{
    /// <summary>
    /// Запрос на получение <typeparamref name="TEntity"/> из базы данных
    /// </summary>
    /// <typeparam name="TEntity">Тип получаемой сущности</typeparam>
    public class EntityRequest<TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Спецификация, на основе которой происходит выборка объектов из базы данных
        /// </summary>
        public ISpecification<TEntity> Specification { get; set; }

        /// <summary>
        /// Создаёт запрос на получение <typeparamref name="TEntity"/>
        /// </summary>
        /// <param name="specification">Спецификация, на основе которой происходит выборка объектов из базы данных</param>
        public EntityRequest(ISpecification<TEntity> specification)
        {
            Specification = specification;
        }
    }
}
