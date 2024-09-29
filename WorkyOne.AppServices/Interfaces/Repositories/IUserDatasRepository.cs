using WorkyOne.Domain.Entities;

namespace WorkyOne.AppServices.Interfaces.Repositories
{
    /// <summary>
    /// Интерфейс репозитория пользовательских данных
    /// </summary>
    public interface IUserDatasRepository
    {
        /// <summary>
        /// Возвращает пользовательские данные
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        public Task<UserDataEntity?> GetAsync(string userId);

        /// <summary>
        /// Обновляет пользовательские данные
        /// </summary>
        /// <param name="data">Обновляемые данные</param>
        /// <returns></returns>
        public Task UpdateAsync(UserDataEntity data);
    }
}
