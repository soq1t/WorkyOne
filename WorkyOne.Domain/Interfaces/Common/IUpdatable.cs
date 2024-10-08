using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkyOne.Domain.Interfaces.Common
{
    /// <summary>
    /// Интерфейс сущности <typeparamref name="TEntity"/>, которая может обновлять свои поля полями иной сущности <typeparamref name="TEntity"/>
    /// </summary>
    /// <typeparam name="TEntity">Класс сущности</typeparam>
    interface IUpdatable<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Обновляет поля сущности полями сущности <paramref name="entity"/>
        /// </summary>
        /// <param name="entity">Сущность с обновлёнными полями</param>
        public void UpdateFields(TEntity entity);
    }
}
