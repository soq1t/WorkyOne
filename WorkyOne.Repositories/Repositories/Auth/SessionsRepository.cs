using WorkyOne.AppServices.Interfaces.Repositories.Auth;
using WorkyOne.Domain.Entities.Auth;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Repositories.Abstractions;

namespace WorkyOne.Repositories.Repositories.Auth
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
        public SessionsRepository(UsersDbContext context)
            : base(context) { }
    }
}
