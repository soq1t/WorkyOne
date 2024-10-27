using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using WorkyOne.Domain.Interfaces.Common;
using WorkyOne.Domain.Interfaces.Specification;

namespace WorkyOne.Domain.Requests.Common
{
    /// <summary>
    /// Пагинированный запрос на получение множества <typeparamref name="TEntity"/> из базы данных
    /// </summary>
    /// <typeparam name="TEntity">Тип получаемой сущности</typeparam>
    public class PaginatedRequest<TEntity> : EntityRequest<TEntity>
        where TEntity : class, IEntity
    {
        public PaginatedRequest(ISpecification<TEntity> specification, int pageIndex, int amount)
            : base(specification)
        {
            if (pageIndex < 0)
                throw new ArgumentException("Value must be greater that zero", nameof(pageIndex));

            if (amount < 0)
                throw new ArgumentException("Value must be greater that zero", nameof(amount));

            PageIndex = pageIndex;
            Amount = amount;
        }

        /// <summary>
        /// Номер страницы с сущностями
        /// </summary>
        [Range(1, int.MaxValue)]
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// Количество сущностей на странице
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Amount { get; set; } = 30;
    }
}
