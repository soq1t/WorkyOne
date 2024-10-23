using System.Linq.Expressions;
using WorkyOne.Domain.Interfaces.Common;

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
        /// Идентификатор <typeparamref name="TEntity"/>, которую требуется получить из базы данных
        /// </summary>
        public string? EntityId { get; set; }

        /// <summary>
        /// Условие, по которой получается сущность
        /// </summary>
        public Expression<Func<TEntity, bool>>? Predicate { get; set; }

        /// <summary>
        /// Создаёт запрос на получение <typeparamref name="TEntity"/>
        /// </summary>
        /// <param name="entityId">Идентификатор запрашиваемой <typeparamref name="TEntity"/></param>
        /// <param name="predicate">Условия, на основании которых будет запрашиваться <typeparamref name="TEntity"/></param>
        public EntityRequest(
            string? entityId = null,
            Expression<Func<TEntity, bool>>? predicate = null
        )
        {
            EntityId = entityId;
            Predicate = predicate;
        }
    }
}
