using WorkyOne.AppServices.Interfaces.Repositories.CRUD;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Domain.Requests.Common;
using WorkyOne.Domain.Requests.Users;

namespace WorkyOne.AppServices.Interfaces.Repositories.Users
{
    /// <summary>
    /// Интерфейс репозитория по работе с <see cref="UserDataEntity"/>
    /// </summary>
    public interface IUserDatasRepository
        : ICrudRepository<UserDataEntity, UserDataRequest, PaginatedRequest<UserDataEntity>> { }
}
