using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Users;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Domain.Requests.Users;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Repositories.Abstractions;

namespace WorkyOne.Repositories.Repositories.Users
{
    /// <summary>
    /// Репозиторий по работе с <see cref="UserDataEntity"/>
    /// </summary>
    public sealed class UserDatasRepository
        : ApplicationBaseRepository<
            UserDataEntity,
            UserDataRequest,
            PaginatedRequest<UserDataEntity>
        >,
            IUserDatasRepository
    {
        public UserDatasRepository(ApplicationDbContext context)
            : base(context) { }

        public override async Task<UserDataEntity?> GetAsync(
            UserDataRequest request,
            CancellationToken cancellation = default
        )
        {
            var userData = await _context.UserDatas.FindAsync(request.EntityId, cancellation);

            if (userData == null && request.Predicate != null)
            {
                userData = await _context.UserDatas.FirstOrDefaultAsync(
                    request.Predicate,
                    cancellation
                );
            }

            return userData;
        }
    }
}
