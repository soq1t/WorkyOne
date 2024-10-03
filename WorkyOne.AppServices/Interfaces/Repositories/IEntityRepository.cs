using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Contracts.Requests.Common;

namespace WorkyOne.AppServices.Interfaces.Repositories
{
    /// <summary>
    /// Интерфейс репозитория базы для работы с <typeparamref name="TEntity"/>
    /// </summary>
    /// <typeparam name="TEntity">Тип данных, с которыми работает репозиторий</typeparam>
    /// <typeparam name="TRequest">Тип запроса на получение данных из базы</typeparam>
    public interface IEntityRepository<TEntity, TRequest>
        where TEntity : class
        where TRequest : RequestBase
    {
        /// <summary>
        /// Возвращает сущность типа <typeparamref name="TEntity"/> из базы данных
        /// </summary>
        /// <param name="request">Запрос для получения сущности</param>
        public Task<TEntity?> GetAsync(TRequest request);

        /// <summary>
        /// Возвращает список сущностей <typeparamref name="TEntity"/> из базы данных
        /// </summary>
        /// <param name="request">Запрос для получения списка сущностей</param>
        public Task<ICollection<TEntity>?> GetManyAsync(TRequest request);

        /// <summary>
        /// Создаёт сущность <typeparamref name="TEntity"/> в базе данных
        /// </summary>
        /// <param name="entity">Создаваемая сущность</param>
        public Task CreateAsync(TEntity entity);

        /// <summary>
        /// Создаёт множество сущностей <typeparamref name="TEntity"/> в базе данных
        /// </summary>
        /// <param name="entities">Список создаваемых сущностей</param>
        public Task CreateManyAsync(ICollection<TEntity> entities);

        /// <summary>
        /// Обновляет сущность <typeparamref name="TEntity"/> в базе данных
        /// </summary>
        /// <param name="entity">Обновляемая сущность</param>
        public Task UpdateAsync(TEntity entity);

        /// <summary>
        /// Обновляет список сущностей <typeparamref name="TEntity"/> в базе данных
        /// </summary>
        /// <param name="entities">Список обновляемых сущностей</param>
        /// <returns></returns>
        public Task UpdateManyAsync(ICollection<TEntity> entities);

        /// <summary>
        /// Удаляет сущность <typeparamref name="TEntity"/> из базы данных согласно её ID
        /// </summary>
        /// <param name="entityId">ID удаляемой сущности</param>
        public Task DeleteAsync(string entityId);

        /// <summary>
        /// Удаляет список сущностей <typeparamref name="TEntity"/> из базы данных согласно их ID
        /// </summary>
        /// <param name="entityIds">Список ID удаляемых сущностей</param>
        public Task DeleteManyAsync(ICollection<string> entityIds);

        /// <summary>
        /// Производит операции удаления, добавления и обновления с сущностями из списка <paramref name="oldEntities"/> относительно сущностей из списка <paramref name="newEntities"/>
        /// </summary>
        /// <param name="oldEntities">Обновляемая коллекция сущностей</param>
        /// <param name="newEntities">Коллекция сущностей, относительно которой происходит обновление</param>
        /// <returns></returns>
        public Task RenewAsync(ICollection<TEntity> oldEntities, ICollection<TEntity> newEntities);
    }
}
