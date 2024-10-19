using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Contracts.Interfaces.Repositories;
using WorkyOne.Contracts.Repositories;
using WorkyOne.Contracts.Requests.Common;
using WorkyOne.Domain.Interfaces.Common;

namespace WorkyOne.AppServices.Interfaces.Repositories.Common
{
    /// <summary>
    /// Интерфейс репозитория для работы с <typeparamref name="TEntity"/> в базе данных
    /// </summary>
    /// <typeparam name="TEntity">Тип данных, с которыми работает репозиторий</typeparam>
    /// <typeparam name="TRequest">Тип запроса на получение данных из базы</typeparam>
    public interface IEntityRepository<TEntity, TRequest>
        where TEntity : IEntity
        where TRequest : IEntityRequest
    {
        /// <summary>
        /// Возвращает сущность типа <typeparamref name="TEntity"/> из базы данных
        /// </summary>
        /// <param name="request">Запрос для получения сущности</param>
        /// <param name="cancellation">Токен отмены задания</param>
        public Task<TEntity?> GetAsync(TRequest request, CancellationToken cancellation = default);

        /// <summary>
        /// Создаёт сущность <typeparamref name="TEntity"/> в базе данных
        /// </summary>
        /// <param name="entity">Создаваемая сущность</param>
        /// <param name="cancellation">Токен отмены задания</param>
        public Task<RepositoryResult> CreateAsync(
            TEntity entity,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Создаёт множество сущностей <typeparamref name="TEntity"/> в базе данных
        /// </summary>
        /// <param name="entities">Список создаваемых сущностей</param>
        /// <param name="cancellation">Токен отмены задания</param>
        public Task<RepositoryResult> CreateManyAsync(
            ICollection<TEntity> entities,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Обновляет сущность <typeparamref name="TEntity"/> в базе данных
        /// </summary>
        /// <param name="entity">Обновляемая сущность</param>
        /// <param name="cancellation">Токен отмены задания</param>
        public Task<RepositoryResult> UpdateAsync(
            TEntity entity,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Обновляет список сущностей <typeparamref name="TEntity"/> в базе данных
        /// </summary>
        /// <param name="entities">Список обновляемых сущностей</param>
        /// <returns></returns>
        /// <param name="cancellation">Токен отмены задания</param>
        public Task<RepositoryResult> UpdateManyAsync(
            ICollection<TEntity> entities,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Удаляет сущность <typeparamref name="TEntity"/> из базы данных согласно её ID
        /// </summary>
        /// <param name="entityId">ID удаляемой сущности</param>
        /// <param name="cancellation">Токен отмены задания</param>
        public Task<RepositoryResult> DeleteAsync(
            string entityId,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Удаляет список сущностей <typeparamref name="TEntity"/> из базы данных согласно их ID
        /// </summary>
        /// <param name="entityIds">Список ID удаляемых сущностей</param>
        /// <param name="cancellation">Токен отмены задания</param>
        public Task<RepositoryResult> DeleteManyAsync(
            ICollection<string> entityIds,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Производит операции удаления, добавления и обновления с сущностями из списка <paramref name="oldEntities"/> относительно сущностей из списка <paramref name="newEntities"/>
        /// </summary>
        /// <param name="oldEntities">Обновляемая коллекция сущностей</param>
        /// <param name="newEntities">Коллекция сущностей, относительно которой происходит обновление</param>
        /// <returns></returns>
        /// <param name="cancellation">Токен отмены задания</param>
        public Task<RepositoryResult> RenewAsync(
            ICollection<TEntity> oldEntities,
            ICollection<TEntity> newEntities,
            CancellationToken cancellation = default
        );
    }
}
