using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Common;
using WorkyOne.Contracts.Enums.Reposistories;
using WorkyOne.Contracts.Interfaces.Repositories;
using WorkyOne.Contracts.Repositories;
using WorkyOne.Domain.Abstractions;
using WorkyOne.Domain.Interfaces.Common;
using WorkyOne.Repositories.Contextes;

namespace WorkyOne.Repositories.Utilities
{
    /// <summary>
    /// Инструмент, предоставляющий стандартные методы для репозиториев
    /// </summary>
    internal static class DefaultRepositoryMethods
    {
        /// <summary>
        /// Стандартная реализация метода RenewAsync для <see cref="IEntityRepository{TEntity, TRequest}"/>
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности, используемой в репозитории</typeparam>
        /// <typeparam name="TRequest">Тип запроса, используемого в репозитории</typeparam>
        /// <param name="repository">Репозиторий для работы с <typeparamref name="TEntity"/></param>
        /// <param name="oldValues">Список <typeparamref name="TEntity"/>, который должен обновляться</param>
        /// <param name="newValues"></param>
        /// <returns></returns>
        public static async Task<RepositoryResult> RenewAsync<TEntity, TRequest>(
            IEntityRepository<TEntity, TRequest> repository,
            ICollection<TEntity> oldValues,
            ICollection<TEntity> newValues
        )
            where TEntity : IEntity
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
        /// Обновляет сущность <typeparamref name="TUpdatable"/> согласно методам интерфейса <see cref="IUpdatable{TEntity}"/>
        /// </summary>
        /// <typeparam name="TUpdatable">Тип обновляемой сущности</typeparam>
        /// <param name="context">Контекст базы данных приложения</param>
        /// <param name="entity">Обновляемая сущность</param>
        public static async Task<RepositoryResult> UpdateAsync<TUpdatable>(
            ApplicationDbContext context,
            TUpdatable entity
        )
            where TUpdatable : EntityBase, IUpdatable<TUpdatable>
        {
            var updated = await context
                .Set<TUpdatable>()
                .FirstOrDefaultAsync(x => x.Id == entity.Id);

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
        /// Обновляет множество сущностей <typeparamref name="TUpdatable"/> согласно методам интерфейса <see cref="IUpdatable{TEntity}"/>
        /// </summary>
        /// <typeparam name="TUpdatable">Тип обновляемых сущностей</typeparam>
        /// <param name="context">Контекст базы данных приложения</param>
        /// <param name="entities">Обновляемые сущности</param>
        /// <returns></returns>
        public static async Task<RepositoryResult> UpdateManyAsync<TUpdatable>(
            ApplicationDbContext context,
            ICollection<TUpdatable> entities
        )
            where TUpdatable : EntityBase, IUpdatable<TUpdatable>
        {
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var result = new RepositoryResult();

                var ids = entities.Select(e => e.Id).ToList();
                var updated = await context
                    .Set<TUpdatable>()
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
