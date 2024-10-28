using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Users;
using WorkyOne.Contracts.Enums.Reposistories;
using WorkyOne.Contracts.Enums.Result;
using WorkyOne.Contracts.Repositories.Result;
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
                return RepositoryResult.Error(ResultType.NotFound, entity.Id, nameof(UserEntity));
            }

            _context.Remove(entity);
            return RepositoryResult.Ok(ResultType.Deleted, entity.Id, nameof(UserEntity));
        }

        public RepositoryResult DeleteMany(IEnumerable<UserEntity> entities)
        {
            var result = new RepositoryResult(true, "Успех");

            foreach (var item in entities)
            {
                if (_context.Entry(item).State == EntityState.Detached)
                {
                    result.AddError(ResultType.NotFound, item.Id, nameof(UserEntity));
                }
                else
                {
                    _context.Users.Remove(item);
                    result.AddSucceed(ResultType.Deleted, item.Id, nameof(UserEntity));
                }
            }

            if (result.SucceedItems.Count == 0)
            {
                result.IsSucceed = false;
                result.Message = "Ошибка";
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
