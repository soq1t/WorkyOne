using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.CRUD;
using WorkyOne.Contracts.Enums.Reposistories;
using WorkyOne.Contracts.Repositories.Common;
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
    public abstract class ApplicationBaseRepository<TEntity, TSingleRequest, TPaginatedRequest>
        : ICrudRepository<TEntity, TSingleRequest, TPaginatedRequest>,
            IDeleteByConditionRepository<TEntity>
        where TEntity : EntityBase
        where TSingleRequest : EntityRequest<TEntity>
        where TPaginatedRequest : PaginatedRequest<TEntity>
    {
        protected readonly ApplicationDbContext _context;

        public ApplicationBaseRepository(ApplicationDbContext context)
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
                return RepositoryResult.CancelationRequested();
            }

            if (existed == null)
            {
                _context.Set<TEntity>().Add(entity);
                return new RepositoryResult(entity.Id);
            }
            else
            {
                return new RepositoryResult(RepositoryErrorType.EntityAlreadyExists, entity.Id);
            }
        }

        public async Task<RepositoryResult> CreateManyAsync(
            IEnumerable<TEntity> entities,
            CancellationToken cancellation = default
        )
        {
            var result = new RepositoryResult();

            var ids = entities.Select(e => e.Id).ToList();

            var existed = await _context
                .Set<TEntity>()
                .Where(e => ids.Contains(e.Id))
                .ToListAsync(cancellation);

            var existedIds = existed.Select(e => e.Id).ToList();

            if (cancellation.IsCancellationRequested)
            {
                return RepositoryResult.CancelationRequested();
            }

            if (existed.Count == 0)
            {
                foreach (var id in existedIds)
                {
                    result.AddError(RepositoryErrorType.EntityAlreadyExists, id);
                }

                if (existed.Count == entities.Count())
                {
                    return result;
                }
            }

            var newEntities = entities.Where(e => !existedIds.Contains(e.Id)).ToList();

            if (cancellation.IsCancellationRequested)
            {
                return RepositoryResult.CancelationRequested();
            }

            _context.Set<TEntity>().AddRange(newEntities);
            result.SucceedIds = newEntities.Select(e => e.Id).ToList();
            return result;
        }

        public RepositoryResult Delete(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                return new RepositoryResult(
                    RepositoryErrorType.EntityNotTrackedByContext,
                    entity.Id
                );
            }

            _context.Remove(entity);
            return new RepositoryResult(entity.Id);
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

            return RepositoryResult.Ok("1");
        }

        public RepositoryResult DeleteMany(IEnumerable<TEntity> entities)
        {
            var result = new RepositoryResult();

            foreach (var item in entities)
            {
                if (_context.Entry(item).State == EntityState.Detached)
                {
                    result.AddError(RepositoryErrorType.EntityNotTrackedByContext, item.Id);
                }
                else
                {
                    _context.Set<TEntity>().Remove(item);
                    result.SucceedIds.Add(item.Id);
                }
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
                return new RepositoryResult(
                    RepositoryErrorType.EntityNotTrackedByContext,
                    entity.Id
                );
            }
            else
            {
                _context.Set<TEntity>().Update(entity);
                return RepositoryResult.Ok(entity.Id);
            }
        }

        public RepositoryResult UpdateMany(IEnumerable<TEntity> entities)
        {
            var result = new RepositoryResult();

            var existed = entities
                .Where(e => _context.Entry(e).State != EntityState.Detached)
                .ToList();

            if (existed.Count < entities.Count())
            {
                var notExisted = entities.Except(existed).ToList();

                result.AddErrors(
                    RepositoryErrorType.EntityNotTrackedByContext,
                    notExisted.Select(e => e.Id)
                );
            }

            result.SucceedIds = existed.Select(e => e.Id).ToList();

            _context.Set<TEntity>().UpdateRange(existed);

            return result;
        }

        //protected IQueryable<TEntity> AddPagination(
        //    IQueryable<TEntity> query,
        //    int pageIndex,
        //    int amount
        //)
        //{
        //    var skip = (pageIndex - 1) * amount;
        //    var take = amount;

        //    return query.Skip(skip).Take(take);
        //}
    }
}
