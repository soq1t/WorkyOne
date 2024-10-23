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
    }
}
