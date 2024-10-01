using WorkyOne.AppServices.Interfaces.Repositories.Requests;
using WorkyOne.Domain.Entities.Schedule;

namespace WorkyOne.AppServices.Interfaces.Repositories
{
    /// <summary>
    /// Интерфейс репозитория шаблонов расписаний
    /// </summary>
    public interface ITemplatesRepository
    {
        /// <summary>
        /// Возвращает шаблон по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор шаблона</param>
        public Task<TemplateEntity?> GetAsync(TemplateRequest request);

        /// <summary>
        /// Возвращает шаблоны указанного пользователя
        /// </summary>
        /// <param name="userDataId">ID пользовательских данных</param>
        /// <returns></returns>
        public Task<List<TemplateEntity>> GetUserTemplatesAsync(TemplateRequest request);

        /// <summary>
        /// Добавляет шаблон в базу данных
        /// </summary>
        /// <param name="template">Добавляемый шаблон</param>
        public Task AddAsync(TemplateEntity template);

        /// <summary>
        /// Обновляет шаблон в базе данных
        /// </summary>
        /// <param name="template">Обновляемый шаблон</param>
        public Task UpdateAsync(TemplateEntity template);

        /// <summary>
        /// Удаляет шаблон из базы данных
        /// </summary>
        /// <param name="id">Идентификатор удаляемого шаблона</param>
        public Task DeleteAsync(string id);
    }
}
