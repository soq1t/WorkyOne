using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkyOne.Contracts.Enums.Reposistories;
using WorkyOne.Contracts.Interfaces.Repositories;
using WorkyOne.Contracts.Repositories;
using WorkyOne.Domain.Abstractions;
using WorkyOne.Domain.Attributes;
using WorkyOne.Domain.Interfaces.Common;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Repositories.Common;

namespace WorkyOne.Repositories.Extensions.Repositories
{
    public static class EntityRepositoryExtensions
    {
        /// <summary>
        /// Расширение <see cref="EntityRepository{TEntity, TRequest}"/>, предоставляющее стандартную реализацию метода RenewAsync
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности, используемой в репозитории</typeparam>
        /// <typeparam name="TRequest">Тип запроса, используемого в репозитории</typeparam>
        /// <param name="repository">Репозиторий для работы с <typeparamref name="TEntity"/></param>
        /// <param name="oldValues">Список <typeparamref name="TEntity"/>, который должен обновляться</param>
        /// <param name="newValues">Список <typeparamref name="TEntity"/>, согласно которому происходит обновление <paramref name="oldValues"/></param>
        /// <returns></returns>
        public static async Task<RepositoryResult> DefaultRenewAsync<TEntity, TRequest>(
            this EntityRepository<TEntity, TRequest> repository,
            ICollection<TEntity> oldValues,
            ICollection<TEntity> newValues
        )
            where TEntity : EntityBase, IUpdatable<TEntity>
            where TRequest : IEntityRequest
        {
            IEnumerable<string> newValuesIds = newValues.Select(n => n.Id);
            IEnumerable<string> oldValuesIds = oldValues.Select(n => n.Id);

            List<TEntity> removing = oldValues.Where(o => !newValuesIds.Contains(o.Id)).ToList();
            List<TEntity> adding = newValues.Where(n => !oldValuesIds.Contains(n.Id)).ToList();
            List<TEntity> updating = newValues.Where(n => oldValuesIds.Contains(n.Id)).ToList();

            var result = new RepositoryResult();

            var operationResult = await repository.DeleteManyAsync(
                removing.Select(r => r.Id).ToList()
            );

            result.AddInfo(operationResult);

            operationResult = await repository.CreateManyAsync(adding);
            result.AddInfo(operationResult);

            operationResult = await repository.UpdateManyAsync(updating);
            result.AddInfo(operationResult);

            return result;
        }

        /// <summary>
        /// Расширение <see cref="EntityRepository{TEntity, TRequest}"/>, предоставляющее стандартную реализацию метода UpdateAsync
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности, используемой в репозитории</typeparam>
        /// <typeparam name="TRequest">Тип запроса, используемого в репозитории</typeparam>
        /// <param name="repository">Репозиторий для работы с <typeparamref name="TEntity"/></param>
        /// <param name="context">Контекст базы данных приложения</param>
        /// <param name="entity">Обновляемая сущность</param>
        /// <returns></returns>
        public static async Task<RepositoryResult> DefaultUpdateAsync<TEntity, TRequest>(
            this EntityRepository<TEntity, TRequest> repository,
            ApplicationDbContext context,
            TEntity entity
        )
            where TEntity : EntityBase, IUpdatable<TEntity>
            where TRequest : IEntityRequest
        {
            var updated = await context.Set<TEntity>().FindAsync(entity.Id);

            if (updated == null)
            {
                return new RepositoryResult(RepositoryErrorType.EntityNotExists, entity.Id);
            }

            updated.UpdateFields(entity);
            context.Update(updated);

            await context.SaveChangesAsync();

            return new RepositoryResult(updated.Id);
        }

        /// <summary>
        /// Расширение <see cref="EntityRepository{TEntity, TRequest}"/>, предоставляющее стандартную реализацию метода UpdateManyAsync
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности, используемой в репозитории</typeparam>
        /// <typeparam name="TRequest">Тип запроса, используемого в репозитории</typeparam>
        /// <param name="repository">Репозиторий для работы с <typeparamref name="TEntity"/></param>
        /// <param name="context">Контекст базы данных приложения</param>
        /// <param name="entities">Список обновляемых сущностей</param>
        /// <returns></returns>
        public static async Task<RepositoryResult> DefaultUpdateManyAsync<TEntity, TRequest>(
            this EntityRepository<TEntity, TRequest> repository,
            ApplicationDbContext context,
            ICollection<TEntity> entities
        )
            where TEntity : EntityBase, IUpdatable<TEntity>
            where TRequest : IEntityRequest
        {
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var result = new RepositoryResult();

                var ids = entities.Select(e => e.Id).ToList();
                var updated = await context
                    .Set<TEntity>()
                    .Where(e => ids.Contains(e.Id))
                    .ToListAsync();
                var updatedIds = updated.Select(e => e.Id).ToList();
                var notExistedIds = ids.Except(updatedIds).ToList();

                notExistedIds.ForEach(id =>
                    result.AddError(RepositoryErrorType.EntityNotExists, id)
                );
                result.SucceedIds.AddRange(updatedIds);

                foreach (var item in updated)
                {
                    item.UpdateFields(entities.First(e => e.Id == item.Id));
                    context.Update(item);
                }

                if (result.IsSuccess)
                {
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                else
                {
                    await transaction.RollbackAsync();
                }

                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
