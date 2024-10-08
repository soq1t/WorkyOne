using WorkyOne.AppServices.Interfaces.Repositories.Common;
using WorkyOne.Contracts.Requests.Common;
using WorkyOne.Domain.Entities.Users;

namespace WorkyOne.AppServices.Interfaces.Repositories.Users
{
    /// <summary>
    /// Интерфейс репозитория по работе с пользовательскими данными
    /// </summary>
    public interface IUsersDataRepository : IEntityRepository<UserDataEntity, UserDataRequest>
    {
    }
}
