using WorkyOne.AppServices.Interfaces.Repositories.CRUD;
using WorkyOne.Domain.Entities.Auth;
using WorkyOne.Domain.Requests.Common;

namespace WorkyOne.AppServices.Interfaces.Repositories.Auth
{
    /// <summary>
    /// Интерфейс репозитория по работе с <see cref="SessionEntity"/>
    /// </summary>
    public interface ISessionsRepository
        : ICrudRepository<
            SessionEntity,
            EntityRequest<SessionEntity>,
            PaginatedRequest<SessionEntity>
        > { }
}
