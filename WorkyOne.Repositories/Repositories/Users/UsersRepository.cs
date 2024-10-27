using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Users;
using WorkyOne.Contracts.Enums.Reposistories;
using WorkyOne.Contracts.Repositories.Common;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Extensions;

namespace WorkyOne.Repositories.Repositories.Users
{
    public sealed class UsersRepository : IUsersRepository
    {
        private readonly UsersDbContext _context;

        public UsersRepository(UsersDbContext context)
        {
            _context = context;
        }

        public RepositoryResult Delete(UserEntity entity)
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

        public RepositoryResult DeleteMany(IEnumerable<UserEntity> entities)
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
                    _context.Users.Remove(item);
                    result.SucceedIds.Add(item.Id);
                }
            }

            return result;
        }

        public Task<UserEntity?> GetAsync(
            EntityRequest<UserEntity> request,
            CancellationToken cancellation = default
        )
        {
            return _context.Users.FirstOrDefaultAsync(
                request.Specification.ToExpression(),
                cancellation
            );
        }

        public Task<List<UserEntity>> GetManyAsync(
            PaginatedRequest<UserEntity> request,
            CancellationToken cancellation = default
        )
        {
            var query = _context.Users.Where(request.Specification.ToExpression());
            query = query.AddPagination(request.PageIndex, request.Amount);

            return query.ToListAsync(cancellation);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
