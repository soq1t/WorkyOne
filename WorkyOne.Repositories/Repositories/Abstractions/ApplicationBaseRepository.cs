using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.CRUD;
using WorkyOne.Contracts.Enums.Result;
using WorkyOne.Contracts.Repositories.Result;
using WorkyOne.Domain.Entities.Abstractions.Common;
using WorkyOne.Domain.Interfaces.Specification;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Extensions;

namespace WorkyOne.Repositories.Repositories.Abstractions
{
    /// <summary>
    /// Репозиторий для выполнения CRUD операций с <typeparamref name="TEntity"/>
    /// </summary>
    /// <typeparam name="TEntity">Тип сущностей, с которыми работает репозиторий</typeparam>
    /// <typeparam name="TSingleRequest">Тип запроса на получение сущности <typeparamref name="TEntity"/></typeparam>
    /// <typeparam name="TPaginatedRequest">Тип запроса на получения множества сущностей <typeparamref name="TEntity"/></typeparam>
    public abstract class ApplicationBaseRepository<
        TContext,
        TEntity,
        TSingleRequest,
        TPaginatedRequest
    >
        : ICrudRepository<TEntity, TSingleRequest, TPaginatedRequest>,
            IDeleteByConditionRepository<TEntity>
        where TContext : DbContext
        where TEntity : EntityBase
        where TSingleRequest : EntityRequest<TEntity>
        where TPaginatedRequest : PaginatedRequest<TEntity>
    {
        protected readonly TContext _context;

        public ApplicationBaseRepository(TContext context)
        {
            _context = context;
        }

        public async Task<RepositoryResult> CreateAsync(
            TEntity entity,
            CancellationToken cancellation = default
        )
        {
            var existed = await _context.Set<TEntity>().FindAsync(entity.Id, cancellation);

            if (cancellation.IsCancellationRequested)
            {
                return RepositoryResult.CancellationRequested();
            }

            if (existed == null)
            {
                _context.Set<TEntity>().Add(entity);
                return RepositoryResult.Ok(ResultType.Created, entity.Id, nameof(TEntity));
            }
            else
            {
                return RepositoryResult.Error(
                    ResultType.AlreadyExisted,
                    entity.Id,
                    nameof(TEntity)
                );
            }
        }

        public async Task<RepositoryResult> CreateManyAsync(
            IEnumerable<TEntity> entities,
            CancellationToken cancellation = default
        )
        {
            var result = new RepositoryResult(false, "Ошибка");

            var ids = entities.Select(e => e.Id).ToList();

            var existed = await _context
                .Set<TEntity>()
                .Where(e => ids.Contains(e.Id))
                .ToListAsync(cancellation);

            var existedIds = existed.Select(e => e.Id).ToList();

            if (cancellation.IsCancellationRequested)
            {
                return RepositoryResult.CancellationRequested();
            }

            if (existed.Count == 0)
            {
                foreach (var id in existedIds)
                {
                    result.AddError(ResultType.AlreadyExisted, id, nameof(TEntity));
                }

                if (existed.Count == entities.Count())
                {
                    return result;
                }
            }

            var newEntities = entities.Where(e => !existedIds.Contains(e.Id)).ToList();

            if (cancellation.IsCancellationRequested)
            {
                return RepositoryResult.CancellationRequested();
            }

            _context.Set<TEntity>().AddRange(newEntities);

            foreach (var entity in newEntities)
            {
                result.AddSucceed(ResultType.Created, entity.Id, nameof(TEntity));
            }

            result.IsSucceed = true;
            result.Message = "Успех!";
            return result;
        }

        public RepositoryResult Delete(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                return RepositoryResult.Error(ResultType.NotFound, entity.Id, nameof(TEntity));
            }

            _context.Remove(entity);
            return RepositoryResult.Ok(ResultType.Deleted, entity.Id, nameof(TEntity));
        }

        public async Task<RepositoryResult> DeleteByConditionAsync(
            ISpecification<TEntity> specification,
            CancellationToken cancellation = default
        )
        {
            await _context
                .Set<TEntity>()
                .Where(specification.ToExpression())
                .ExecuteDeleteAsync(cancellation);

            return RepositoryResult.Ok();
        }

        public RepositoryResult DeleteMany(IEnumerable<TEntity> entities)
        {
            var result = new RepositoryResult(true, "Успех");

            foreach (var item in entities)
            {
                if (_context.Entry(item).State == EntityState.Detached)
                {
                    result.AddError(ResultType.NotFound, item.Id, nameof(TEntity));
                }
                else
                {
                    _context.Set<TEntity>().Remove(item);
                    result.AddSucceed(ResultType.Deleted, item.Id, nameof(TEntity));
                }
            }

            if (result.SucceedItems.Count == 0)
            {
                result.IsSucceed = false;
                result.Message = "Ошибка";
            }
            return result;
        }

        public virtual Task<TEntity?> GetAsync(
            TSingleRequest request,
            CancellationToken cancellation = default
        )
        {
            return _context
                .Set<TEntity>()
                .FirstOrDefaultAsync(request.Specification.ToExpression(), cancellation);
        }

        public virtual Task<List<TEntity>> GetManyAsync(
            TPaginatedRequest request,
            CancellationToken cancellation = default
        )
        {
            IQueryable<TEntity> query = _context
                .Set<TEntity>()
                .Where(request.Specification.ToExpression());

            query = query.AddPagination(request.PageIndex, request.Amount);

            return query.ToListAsync(cancellation);
        }

        public Task SaveChangesAsync(CancellationToken cancellation = default)
        {
            return _context.SaveChangesAsync(cancellation);
        }

        public RepositoryResult Update(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                return RepositoryResult.Error(ResultType.NotFound, entity.Id, nameof(TEntity));
            }
            else
            {
                _context.Set<TEntity>().Update(entity);
                return RepositoryResult.Ok(ResultType.Updated, entity.Id, nameof(TEntity));
            }
        }

        public RepositoryResult UpdateMany(IEnumerable<TEntity> entities)
        {
            var result = RepositoryResult.Ok();

            var existed = entities
                .Where(e => _context.Entry(e).State != EntityState.Detached)
                .ToList();

            if (existed.Count < entities.Count())
            {
                var notExisted = entities.Except(existed).ToList();

                foreach (var item in notExisted)
                {
                    result.AddError(ResultType.NotFound, item.Id, nameof(TEntity));
                }
            }

            _context.Set<TEntity>().UpdateRange(existed);
            existed.ForEach(x => result.AddError(ResultType.Updated, x.Id, nameof(TEntity)));

            return result;
        }
    }
}
