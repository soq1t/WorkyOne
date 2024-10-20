using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Common;
using WorkyOne.Contracts.Enums.Reposistories;
using WorkyOne.Contracts.Interfaces.Repositories;
using WorkyOne.Contracts.Repositories;
using WorkyOne.Domain.Abstractions;
using WorkyOne.Domain.Interfaces.Common;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Extensions.Repositories;

namespace WorkyOne.Repositories.Repositories.Common
{
    /// <summary>
    /// Абстрактный репозиторий для работы с <typeparamref name="TEntity"/> в базе данных
    /// </summary>
    /// <typeparam name="TEntity">Тип данных, с которыми работает репозиторий</typeparam>
    /// <typeparam name="TRequest">Тип запроса на получение данных из базы</typeparam>
    public abstract class EntityRepository<TEntity, TRequest> : IEntityRepository<TEntity, TRequest>
        where TEntity : EntityBase, IUpdatable<EntityBase>
        where TRequest : IEntityRequest
    {
        protected readonly IBaseRepository _baseRepo;
        protected readonly ApplicationDbContext _context;

        public EntityRepository(IBaseRepository baseRepo, ApplicationDbContext context)
        {
            _baseRepo = baseRepo;
            _context = context;
        }

        public Task<RepositoryResult> CreateAsync(
            TEntity entity,
            CancellationToken cancellation = default
        )
        {
            return _baseRepo.CreateAsync(entity, cancellation);
        }

        public Task<RepositoryResult> CreateManyAsync(
            ICollection<TEntity> entities,
            CancellationToken cancellation = default
        )
        {
            return _baseRepo.CreateManyAsync(entities, cancellation);
        }

        public Task<RepositoryResult> DeleteAsync(
            string entityId,
            CancellationToken cancellation = default
        )
        {
            return _baseRepo.DeleteAsync<TEntity>(entityId, cancellation);
        }

        public Task<RepositoryResult> DeleteManyAsync(
            ICollection<string> entityIds,
            CancellationToken cancellation = default
        )
        {
            return _baseRepo.DeleteManyAsync<TEntity>(entityIds, cancellation);
        }

        public virtual Task<TEntity?> GetAsync(
            TRequest request,
            CancellationToken cancellation = default
        )
        {
            return _context
                .Set<TEntity>()
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellation);
        }

        public async Task<RepositoryResult> RenewAsync(
            ICollection<TEntity> oldEntities,
            ICollection<TEntity> newEntities,
            CancellationToken cancellation = default
        )
        {
            var newValuesIds = newEntities.Select(n => n.Id);
            var oldValuesIds = oldEntities.Select(n => n.Id);

            var removing = oldEntities.Where(o => !newValuesIds.Contains(o.Id)).ToList();
            var adding = newEntities.Where(n => !oldValuesIds.Contains(n.Id)).ToList();
            var updating = newEntities.Where(n => oldValuesIds.Contains(n.Id)).ToList();

            var result = new RepositoryResult();

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (cancellation.IsCancellationRequested)
                {
                    return result;
                }
                var operationResult = await DeleteManyAsync(
                    removing.Select(r => r.Id).ToList(),
                    cancellation
                );

                result.AddInfo(operationResult);

                if (cancellation.IsCancellationRequested)
                {
                    await transaction.RollbackAsync();
                    return new RepositoryResult();
                }
                operationResult = await CreateManyAsync(adding, cancellation);
                result.AddInfo(operationResult);

                if (cancellation.IsCancellationRequested)
                {
                    await transaction.RollbackAsync();
                    return new RepositoryResult();
                }
                operationResult = await UpdateManyAsync(updating, cancellation);
                result.AddInfo(operationResult);

                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public virtual async Task<RepositoryResult> UpdateAsync(
            TEntity entity,
            CancellationToken cancellation = default
        )
        {
            var updated = await _context.Set<TEntity>().FindAsync(entity.Id);

            if (updated == null)
            {
                return new RepositoryResult(RepositoryErrorType.EntityNotExists, entity.Id);
            }

            updated.UpdateFields(entity);
            _context.Update(updated);

            if (cancellation.IsCancellationRequested)
            {
                return new RepositoryResult(RepositoryErrorType.OperationCanceled);
            }
            await _context.SaveChangesAsync(cancellation);

            return new RepositoryResult(updated.Id);
        }

        public virtual async Task<RepositoryResult> UpdateManyAsync(
            ICollection<TEntity> entities,
            CancellationToken cancellation = default
        )
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var result = new RepositoryResult();

                var ids = entities.Select(e => e.Id).ToList();
                var updated = await _context
                    .Set<TEntity>()
                    .Where(e => ids.Contains(e.Id))
                    .ToListAsync(cancellation);

                var updatedIds = updated.Select(e => e.Id).ToList();
                var notExistedIds = ids.Except(updatedIds).ToList();

                notExistedIds.ForEach(id =>
                    result.AddError(RepositoryErrorType.EntityNotExists, id)
                );
                result.SucceedIds.AddRange(updatedIds);

                if (cancellation.IsCancellationRequested)
                {
                    return new RepositoryResult(RepositoryErrorType.OperationCanceled);
                }

                foreach (var item in updated)
                {
                    item.UpdateFields(entities.First(e => e.Id == item.Id));
                    _context.Update(item);
                }

                if (cancellation.IsCancellationRequested)
                {
                    await transaction.RollbackAsync();
                    return new RepositoryResult(RepositoryErrorType.OperationCanceled);
                }

                if (result.IsSuccess)
                {
                    await _context.SaveChangesAsync(cancellation);
                    await transaction.CommitAsync(cancellation);
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
