using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Domain.Entities.Schedule;

namespace WorkyOne.AppServices.Interfaces.Repositories
{
    /// <summary>
    /// Интерфейс репозитория по управлению повторениями смен в шаблонах
    /// </summary>
    public interface IRepititionsRepository
    {
        /// <summary>
        /// Возхвращает все повторения указанного шаблона
        /// </summary>
        /// <param name="templateId">ID шаблона</param>
        /// <returns></returns>
        public Task<List<RepititionEntity>> GetAsync(string templateId);

        /// <summary>
        /// Обновляет повторения в указанном шаблоне
        /// </summary>
        /// <param name="templateId">ID шаблона</param>
        /// <param name="repititions">Список повторений</param>
        /// <returns></returns>
        public Task UpdateAsync(string templateId, List<RepititionEntity> repititions);

        /// <summary>
        /// Удаляет повторения в указанном шаблоне
        /// </summary>
        /// <param name="templateId">ID шаблона</param>
        /// <returns></returns>
        public Task DeleteAsync(string templateId);
    }
}
