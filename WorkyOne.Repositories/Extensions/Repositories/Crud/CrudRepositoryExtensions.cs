using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.CRUD;
using WorkyOne.Domain.Interfaces.Common;
using WorkyOne.Domain.Requests.Common;

namespace WorkyOne.Repositories.Extensions.Repositories.Crud
{
    public static class CrudRepositoryExtensions
    {
        public static async Task<TEntity?> DefaultGetAsync<
            TEntity,
            TSingleRequest,
            TPaginatedRequest,
            TContext
        >(
            this IGetRepository<TEntity, TSingleRequest, TPaginatedRequest> getRepo,
            TContext context,
            TSingleRequest request,
            CancellationToken cancellation = default
        )
            where TEntity : class, IEntity
            where TContext : DbContext
            where TSingleRequest : EntityRequest<TEntity>
            where TPaginatedRequest : PaginatedRequest<TEntity>
        {
            if (request.EntityId != null)
            {
                return await context.Set<TEntity>().FindAsync(request.EntityId, cancellation);
            }
            else if (request.Predicate != null)
            {
                return await context
                    .Set<TEntity>()
                    .FirstOrDefaultAsync(request.Predicate, cancellation);
            }
            else
            {
                return null;
            }
        }
    }
}
