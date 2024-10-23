using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Domain.Interfaces.Common;

namespace WorkyOne.Domain.Requests.Common
{
    /// <summary>
    /// Пагинированный запрос на получение множества <typeparamref name="TEntity"/> из базы данных
    /// </summary>
    /// <typeparam name="TEntity">Тип получаемой сущности</typeparam>
    public class PaginatedRequest<TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// Условие, по которому <typeparamref name="TEntity"/> выбирается из базы данных
        /// </summary>
        public Expression<Func<TEntity, bool>> Predicate { get; set; } = (x) => true;

        /// <summary>
        /// Номер страницы с сущностями
        /// </summary>
        [Range(1, int.MaxValue)]
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// Количество <typeparamref name="TEntity"/> на странице
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Amount { get; set; } = 30;
    }
}
