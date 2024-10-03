using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Domain.Entities.Schedule;

namespace WorkyOne.AppServices.Interfaces.Repositories.Schedule
{
    /// <summary>
    /// Интерфейс репозитория по работе с шаблонами
    /// </summary>
    public interface ITemplatesRepository
    {
        /// <summary>
        /// Возвращает шаблон из базы данных
        /// </summary>
        /// <param name="templateId">ID запрашиваемого шаблона</param>
        /// <returns></returns>
        public Task<TemplateEntity?> GetAsync(string templateId);

        /// <summary>
        /// Возвращает их базы данных шаблон, связанный с указанным расписанием
        /// </summary>
        /// <param name="scheduleId">ID расписания</param>
        /// <returns></returns>
        public Task<TemplateEntity?> GetByScheduleIdAsync(string scheduleId);

        /// <summary>
        /// Создаёт шаблон в базе данных
        /// </summary>
        /// <param name="template">Создаваемый шаблон</param>
        /// <returns></returns>
        public Task CreateAsync(TemplateEntity template);

        /// <summary>
        /// Обновляет шаблон в базе данных
        /// </summary>
        /// <param name="template">Обновляемый шаблон</param>
        /// <returns></returns>
        public Task UpdateAsync(TemplateEntity template);

        /// <summary>
        /// Удаляет шаблон из базы данных
        /// </summary>
        /// <param name="templateId">ID удаляемого шаблона</param>
        /// <returns></returns>
        public Task DeleteAsync(string templateId);
    }
}
