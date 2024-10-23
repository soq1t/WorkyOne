using WorkyOne.AppServices.Interfaces.Repositories.CRUD;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Domain.Requests.Common;

namespace WorkyOne.AppServices.Interfaces.Repositories.Users
{
    /// <summary>
    /// Интерфейс репозитория по работе с пользователями
    /// </summary>
    public interface IUsersRepository
        : IGetRepository<UserEntity, EntityRequest<UserEntity>, PaginatedRequest<UserEntity>>,
            IDeleteRepository<UserEntity> { }
}
