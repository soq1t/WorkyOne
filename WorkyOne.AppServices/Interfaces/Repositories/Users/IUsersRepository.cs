using WorkyOne.AppServices.Interfaces.Repositories.Common;
using WorkyOne.Contracts.Requests.Common;
using WorkyOne.Domain.Entities.Users;

namespace WorkyOne.AppServices.Interfaces.Repositories.Users
{
    /// <summary>
    /// Интерфейс репозитория по работе с пользователями
    /// </summary>
    public interface IUsersRepository : IEntityRepository<UserEntity, UserRequest>
    {

    }
}
