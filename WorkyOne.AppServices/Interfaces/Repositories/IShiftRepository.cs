using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Domain.Entities.Schedule;

namespace WorkyOne.AppServices.Interfaces.Repositories
{
    /// <summary>
    /// Интерфейс репозитория рабочих смен
    /// </summary>
    public interface IShiftRepository
    {
        /// <summary>
        /// Возвращает смену по её ID
        /// </summary>
        /// <param name="id">ID смены</param>
        public Task<ShiftEntity?> GetAsync(string id);

        /// <summary>
        /// Возвращает все смены, входящие в указанный шаблон
        /// </summary>
        /// <param name="templateId">ID шаблона</param>
        /// <returns></returns>
        public Task<List<ShiftEntity>> GetByTemplateIdAsync(string templateId);

        /// <summary>
        /// Обновляет смену в базе данных
        /// </summary>
        /// <param name="shift">Обновляемая смена</param>
        public Task UpdateAsync(ShiftEntity shift);

        /// <summary>
        /// Обновляет заданные смены в базе
        /// </summary>
        /// <param name="shifts">Список обновляемых смен</param>
        /// <returns></returns>
        public Task UpdateAsync(List<ShiftEntity> shifts);

        /// <summary>
        /// Удаляет смену из базы данных
        /// </summary>
        /// <param name="id">ID удаляемой смены</param>
        public Task DeleteAsync(string id);

        /// <summary>
        /// Удаляет несколько смен из базы данных
        /// </summary>
        /// <param name="ids">Список ID удаляемых смен</param>
        public Task DeleteAsync(List<string> ids);

        /// <summary>
        /// Добавляет смену в базу данных
        /// </summary>
        /// <param name="shift">Добавляемая смена</param>
        public Task AddAsync(ShiftEntity shift);

        /// <summary>
        /// Добавляет указанные смены в базу данных
        /// </summary>
        /// <param name="shifts">Добавляемые смены</param>
        public Task AddAsync(List<ShiftEntity> shifts);
    }
}
