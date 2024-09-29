using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkyOne.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс, описывающий сущность, которая может обновить свои поля
    /// </summary>
    /// <typeparam name="T">Класс сущности</typeparam>
    public interface IUpdatable<T>
        where T : class
    {
        /// <summary>
        /// Обновляет поля сущности на основе полей экземпляра, передаваемого в метод
        /// </summary>
        /// <param name="entity">Экземляр, поля которого содержат обновлённую информацию</param>
        public void Update(T entity);
    }
}
