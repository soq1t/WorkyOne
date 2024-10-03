using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Domain.Entities.Schedule.Shifts;

namespace WorkyOne.AppServices.Interfaces.Repositories.Schedule
{
    /// <summary>
    /// Интерфейс репозитория по работе со сменами, используемыми в шаблонах
    /// </summary>
    public interface ITemplatedShiftsRepository
    {
        /// <summary>
        /// Возвращает смену из базы данных
        /// </summary>
        /// <param name="shiftId">ID запрашиваемой смены</param>
        public Task<TemplatedShiftEntity?> GetAsync(string shiftId);

        /// <summary>
        /// Возвращает список смен, которые включает указанный шаблон
        /// </summary>
        /// <param name="templateId">ID шаблона</param>
        /// <returns></returns>
        public Task<List<TemplatedShiftEntity>>? GetByTemplateIdAsync(string templateId);

        /// <summary>
        /// Создаёт смену в базе данных
        /// </summary>
        /// <param name="shift">Создаваемая смена</param>
        /// <returns></returns>
        public Task CreateAsync(TemplatedShiftEntity shift);

        /// <summary>
        /// Обновляет смену в базе данных
        /// </summary>
        /// <param name="shift">Обновляемая смена</param>
        /// <returns></returns>
        public Task UpdateAsync(TemplatedShiftEntity shift);

        /// <summary>
        /// Удаляет смену из базы данных
        /// </summary>
        /// <param name="shiftId">ID удаляемой смены</param>
        /// <returns></returns>
        public Task DeleteAsync(string shiftId);
    }
}
