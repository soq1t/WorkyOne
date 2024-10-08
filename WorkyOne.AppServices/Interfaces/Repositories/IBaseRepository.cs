﻿using WorkyOne.Contracts.Repositories;
using WorkyOne.Domain.Abstractions;
using WorkyOne.Domain.Interfaces;

namespace WorkyOne.AppServices.Interfaces.Repositories
{
    /// <summary>
    /// Интерфейс базового репозитория
    /// </summary>
    public interface IBaseRepository
    {
        /// <summary>
        /// Возвращает из базы сущность типа <typeparamref name="TEntity"/> согласно её идентификатору
        /// </summary>
        /// <typeparam name="TEntity">Тип возвращаемой сущности</typeparam>
        /// <param name="entityId">Идентификатор запрашиваемой сущности</param>
        public Task<TEntity?> GetByIdAsync<TEntity>(string entityId)
            where TEntity : EntityBase;

        /// <summary>
        /// Создаёт в базе данных сущность типа <typeparamref name="TEntity"/>
        /// </summary>
        /// <typeparam name="TEntity">Тип создаваемой сущности</typeparam>
        /// <param name="entity">Создаваемая сущность</param>
        public Task<RepositoryResult> CreateAsync<TEntity>(TEntity entity)
            where TEntity : EntityBase;

        /// <summary>
        /// Создаёт в базе данных множество сущностей типа <typeparamref name="TEntity"/>
        /// </summary>
        /// <typeparam name="TEntity">Тип создаваемой сущности</typeparam>
        /// <param name="entities">Список создаваемых сущностей</param>
        public Task<RepositoryResult> CreateManyAsync<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : EntityBase;

        /// <summary>
        /// Удаляет из базы данных сущность типа <typeparamref name="TEntity"/>
        /// </summary>
        /// <typeparam name="TEntity">Тип удаляемой сущности</typeparam>
        /// <param name="entityId">Идентификатор удаляемой сущности</param>
        public Task<RepositoryResult> DeleteAsync<TEntity>(string entityId)
            where TEntity : EntityBase;

        /// <summary>
        /// Удаляет из базы данных множество сущностей типа <typeparamref name="TEntity"/>
        /// </summary>
        /// <typeparam name="TEntity">Тип удаляемых сущностей</typeparam>
        /// <param name="entitiesIds">Список идентификаторов удаляемых сущностей</param>
        /// <returns></returns>
        public Task<RepositoryResult> DeleteManyAsync<TEntity>(IEnumerable<string> entitiesIds)
            where TEntity : EntityBase;
    }
}
