using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Common;
using WorkyOne.Contracts.Enums.Reposistories;
using WorkyOne.Contracts.Repositories;
using WorkyOne.Domain.Abstractions;
using WorkyOne.Repositories.Contextes;

namespace WorkyOne.Repositories.Repositories.Common
{
    /// <summary>
    /// Базовый репозиторий приложения
    /// </summary>
    public class ApplicationBaseRepository : IBaseRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationBaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RepositoryResult> CreateAsync<TEntity>(
            TEntity entity,
            CancellationToken cancellation = default
        )
            where TEntity : EntityBase
        {
            var result = new RepositoryResult();

            var existed = await _context
                .Set<TEntity>()
                .AnyAsync(e => e.Id == entity.Id, cancellation);

            if (existed)
            {
                result.AddError(RepositoryErrorType.EntityAlreadyExists, entity.Id);
            }
            else
            {
                if (cancellation.IsCancellationRequested)
                {
                    return new RepositoryResult(RepositoryErrorType.OperationCanceled);
                }
                _context.Set<TEntity>().Add(entity);
                await _context.SaveChangesAsync(cancellation);
                result.SucceedIds.Add(entity.Id);
            }

            return result;
        }

        public async Task<RepositoryResult> CreateManyAsync<TEntity>(
            IEnumerable<TEntity> entities,
            CancellationToken cancellation = default
        )
            where TEntity : EntityBase
        {
            var result = new RepositoryResult();

            var ids = entities.Select(e => e.Id).ToList();
            var existed = await _context
                .Set<TEntity>()
                .Where(e => ids.Contains(e.Id))
                .ToListAsync(cancellation);

            var existedIds = existed.Select(e => e.Id).ToList();
            var newIds = ids.Except(existedIds);

            existedIds.ForEach(x => result.AddError(RepositoryErrorType.EntityAlreadyExists, x));

            if (newIds.Any())
            {
                if (cancellation.IsCancellationRequested)
                {
                    return new RepositoryResult(RepositoryErrorType.OperationCanceled);
                }
                _context.Set<TEntity>().AddRange(entities.Where(e => newIds.Contains(e.Id)));
                await _context.SaveChangesAsync(cancellation);
                result.SucceedIds.AddRange(newIds);
            }

            return result;
        }

        public async Task<RepositoryResult> DeleteAsync<TEntity>(
            string entityId,
            CancellationToken cancellation = default
        )
            where TEntity : EntityBase
        {
            var result = new RepositoryResult();

            var deleted = await _context.Set<TEntity>().FindAsync(entityId, cancellation);

            if (deleted != null)
            {
                if (cancellation.IsCancellationRequested)
                {
                    return new RepositoryResult(RepositoryErrorType.OperationCanceled);
                }
                _context.Remove(deleted);
                await _context.SaveChangesAsync(cancellation);
                result.SucceedIds.Add(entityId);
            }
            else
            {
                result.AddError(RepositoryErrorType.EntityNotExists, entityId);
            }

            return result;
        }

        public async Task<RepositoryResult> DeleteManyAsync<TEntity>(
            IEnumerable<string> entitiesIds,
            CancellationToken cancellation = default
        )
            where TEntity : EntityBase
        {
            var result = new RepositoryResult();

            var deleted = await _context
                .Set<TEntity>()
                .Where(e => entitiesIds.Contains(e.Id))
                .ToListAsync(cancellation);

            var existedIds = deleted.Select(e => e.Id).ToList();
            var notExistedIds = entitiesIds.Except(existedIds).ToList();

            notExistedIds.ForEach(x => result.AddError(RepositoryErrorType.EntityNotExists, x));

            if (existedIds.Any())
            {
                if (cancellation.IsCancellationRequested)
                {
                    return new RepositoryResult(RepositoryErrorType.OperationCanceled);
                }
                _context.Remove(deleted);
                await _context.SaveChangesAsync(cancellation);
                result.SucceedIds.AddRange(existedIds);
            }

            return result;
        }

        public Task<TEntity?> GetByIdAsync<TEntity>(
            string entityId,
            CancellationToken cancellation = default
        )
            where TEntity : EntityBase
        {
            return _context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == entityId, cancellation);
        }
    }
}
