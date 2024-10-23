using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Users;
using WorkyOne.Contracts.Enums.Reposistories;
using WorkyOne.Contracts.Repositories.Common;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Extensions.Repositories.Crud;

namespace WorkyOne.Repositories.Repositories.Users
{
    public sealed class UsersRepository : IUsersRepository
    {
        private readonly UsersDbContext _context;

        public UsersRepository(UsersDbContext context)
        {
            _context = context;
        }

        public async Task<RepositoryResult> DeleteAsync(
            string entityId,
            CancellationToken cancellation = default
        )
        {
            var deleted = await _context.Users.FindAsync(entityId, cancellation);

            if (cancellation.IsCancellationRequested)
            {
                return RepositoryResult.CancelationRequested();
            }

            if (deleted == null)
            {
                return RepositoryResult.CancelationRequested();
            }
            else
            {
                _context.Remove(deleted);
                return new RepositoryResult(entityId);
            }
        }

        public async Task<RepositoryResult> DeleteManyAsync(
            IEnumerable<string> entitiesIds,
            CancellationToken cancellation = default
        )
        {
            var result = new RepositoryResult();

            var existed = await _context
                .Users.Where(e => entitiesIds.Contains(e.Id))
                .ToListAsync(cancellation);

            if (cancellation.IsCancellationRequested)
            {
                return RepositoryResult.CancelationRequested();
            }

            var existedIds = existed.Select(e => e.Id).ToList();
            result.SucceedIds = existedIds;

            if (existedIds.Count < entitiesIds.Count())
            {
                var notExistedIds = entitiesIds.Except(existedIds);

                foreach (var id in notExistedIds)
                {
                    result.AddError(RepositoryErrorType.EntityNotExists, id);
                }
            }

            _context.Users.RemoveRange(existed);
            return result;
        }

        public Task<UserEntity?> GetAsync(
            EntityRequest<UserEntity> request,
            CancellationToken cancellation = default
        )
        {
            return this.DefaultGetAsync(_context, request, cancellation);

            //if (request.EntityId != null)
            //{
            //    return await _context.Users.FindAsync(request.EntityId, cancellation);
            //}
            //else if (request.Predicate != null)
            //{
            //    return await _context.Users.FirstOrDefaultAsync(request.Predicate, cancellation);
            //}
            //else
            //{
            //    return null;
            //}
        }

        public Task<List<UserEntity>> GetManyAsync(
            PaginatedRequest<UserEntity> request,
            CancellationToken cancellation = default
        )
        {
            var skip = (request.PageIndex - 1) * request.Amount;
            var take = request.Amount;

            return _context
                .Users.Where(request.Predicate)
                .Skip(skip)
                .Take(take)
                .ToListAsync(cancellation);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
