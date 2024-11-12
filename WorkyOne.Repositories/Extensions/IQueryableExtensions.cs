using WorkyOne.Domain.Interfaces.Common;
using WorkyOne.Domain.Requests.Common;

namespace WorkyOne.Repositories.Extensions
{
    /// <summary>
    /// Методы расширения для <see cref="IQueryable{T}"/>
    /// </summary>
    public static class IQueryableExtensions
    {
        /// <summary>
        /// Добавляет пагинацию в указанный <see cref="IQueryable{T}"/>
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="query">Объект, к которому добавляется пагинация</param>
        /// <param name="pageIndex">Номер страницы</param>
        /// <param name="amount">Количество элементов на странице</param>
        public static IQueryable<T> AddPagination<T>(
            this IQueryable<T> query,
            int pageIndex,
            int amount
        )
            where T : class, IEntity
        {
            if (pageIndex < 0)
            {
                throw new ArgumentException($"{nameof(query)} must be greater than zero");
            }
            if (amount < 0)
            {
                throw new ArgumentException($"{nameof(amount)} must be greater than zero");
            }

            var skip = (pageIndex - 1) * amount;
            var take = amount;

            return query.OrderBy(x => x.Id).Skip(skip).Take(take);
        }

        /// <summary>
        /// Добавляет пагинацию в указанный <see cref="IQueryable{T}"/>
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="query">Объект, к которому добавляется пагинация</param>
        /// <param name="request">Пагинированный запрос на получение <typeparamref name="T"/></param>
        public static IQueryable<T> AddPagination<T>(
            this IQueryable<T> query,
            PaginatedRequest<T> request
        )
            where T : class, IEntity => AddPagination(query, request.PageIndex, request.Amount);
    }
}
