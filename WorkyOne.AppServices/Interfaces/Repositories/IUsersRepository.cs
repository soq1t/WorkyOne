using WorkyOne.Domain.Entities;

namespace WorkyOne.AppServices.Interfaces.Repositories
{
    /// <summary>
    /// Интефрейс репозитория пользователей приложения
    /// </summary>
    public interface IUsersRepository
    {
        /// <summary>
        /// Возвращает список всех пользователей
        /// </summary>
        public Task<List<UserEntity>> GetAllAsync();

        /// <summary>
        /// Возвращает пользователя по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        public Task<UserEntity?> GetUserByIdAsync(string id);

        /// <summary>
        /// Возвращает пользователя по его юзернейму
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        /// <returns></returns>
        public Task<UserEntity?> GetUserByUsernameAsync(string username);
    }
}
