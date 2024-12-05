using WorkyOne.AppServices.Interfaces.Repositories.Auth;
using WorkyOne.AppServices.Interfaces.Utilities;
using WorkyOne.Domain.Entities.Auth;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Repositories.Repositories.Abstractions;
using WorkyOne.Repositories.Users.Contextes;

namespace WorkyOne.Repositories.Users.Repositories.Auth
{
    public class SessionsRepository
        : ApplicationBaseRepository<
            UsersDbContext,
            SessionEntity,
            EntityRequest<SessionEntity>,
            PaginatedRequest<SessionEntity>
        >,
            ISessionsRepository
    {
        public SessionsRepository(UsersDbContext context, IEntityUpdateUtility entityUpdater)
            : base(context, entityUpdater) { }
    }
}
