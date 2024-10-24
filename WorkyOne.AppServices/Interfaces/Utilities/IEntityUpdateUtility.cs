using AutoMapper;
using WorkyOne.Contracts.DTOs.Abstractions;
using WorkyOne.Domain.Interfaces.Common;

namespace WorkyOne.AppServices.Interfaces.Utilities
{
    /// <summary>
    /// Интерфейс сервиса по обновлению сущностей
    /// </summary>
    public interface IEntityUpdateUtility
    {
        /// <summary>
        /// Обновляет поля сущности <paramref name="target"/> полями сущности <paramref name="source"/>
        /// </summary>
        /// <typeparam name="TEntity">Тип обновляемой сущности</typeparam>
        /// <param name="target">Сущность, поля которой будут обновляться</param>
        /// <param name="source">Сущность, полями которой обновляются поля <paramref name="source"/></param>
        /// <param name="propNames">Список полей, которые необходимо обновить. Если равен null, то обновляет все поля</param>
        public void Update<TEntity>(
            TEntity target,
            TEntity source,
            IEnumerable<string>? propNames = null
        )
            where TEntity : class, IEntity;
    }
}
