using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Domain.Entities.Schedule;

namespace WorkyOne.AppServices.Interfaces.Repositories
{
    /// <summary>
    /// Интерфейс репозитория по управлению "однодневными" сменами
    /// </summary>
    public interface ISingleDayShiftRepository
    {
        /// <summary>
        /// Возвращает "однодневную" смену
        /// </summary>
        /// <param name="id">ID "однодневной" смены</param>
        /// <returns></returns>
        public Task<SingleDayShiftEntity?> GetAsync(string id);

        /// <summary>
        /// Возвращает список "однодневных" смен для указанного шаблона
        /// </summary>
        /// <param name="templateId">ID шаблона</param>
        /// <returns></returns>
        public Task<List<SingleDayShiftEntity>> GetByTemplateIdAsync(string templateId);

        /// <summary>
        /// Добавляет "однодневную" смену в базу данных
        /// </summary>
        /// <param name="entity">Добавляемая "одноднвная" смена</param>
        /// <returns></returns>
        public Task AddAsync(SingleDayShiftEntity entity);

        /// <summary>
        /// Добавляет несколько "однодневных" смен в базу данных
        /// </summary>
        /// <param name="entities">Список добавляемых "однодневных" смен</param>
        /// <returns></returns>
        public Task AddAsync(List<SingleDayShiftEntity> entities);

        /// <summary>
        /// Обновляет "однодневную" смену в базе данных
        /// </summary>
        /// <param name="entity">Обновляемая "однодневная" смена</param>
        /// <returns></returns>
        public Task UpdateAsync(SingleDayShiftEntity entity);

        /// <summary>
        /// Обновляет несколько "однодневных" смен в базе данных
        /// </summary>
        /// <param name="entities">Список обновляемых "однодневных" смен</param>
        /// <returns></returns>
        public Task UpdateAsync(List<SingleDayShiftEntity> entities);

        /// <summary>
        /// Удаляет "однодневную" смену из базы данных
        /// </summary>
        /// <param name="id">ID удаляемой "однодневной" смены</param>
        /// <returns></returns>
        public Task DeleteAsync(string id);

        /// <summary>
        /// Удаляет список "однодневных" смен из базы данных
        /// </summary>
        /// <param name="ids">Список ID удаляемых "однодневных" смен</param>
        /// <returns></returns>
        public Task DeleteAsync(List<string> ids);
    }
}
