using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <returns></returns>
        public static IQueryable<T> AddPagination<T>(
            this IQueryable<T> query,
            int pageIndex,
            int amount
        )
            where T : class
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

            return query.Skip(skip).Take(take);
        }
    }
}
