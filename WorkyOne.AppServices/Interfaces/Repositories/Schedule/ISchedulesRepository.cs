using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Requests;
using WorkyOne.Domain.Entities.Schedule;

namespace WorkyOne.AppServices.Interfaces.Repositories.Schedule
{
    /// <summary>
    /// Интерфейс репозитория по работе с Schedule
    /// </summary>
    public interface ISchedulesRepository
    {
        /// <summary>
        /// Возвращает расписание из базы данных
        /// </summary>
        /// <param name="request">Запрос на получение расписания</param>
        /// <returns></returns>
        public Task<ScheduleEntity?> GetAsync(ScheduleRequest request);

        /// <summary>
        /// Возвращает список расписаний, относящихся к указанному пользователю
        /// </summary>
        /// <param name="scheduleRequest">Запрос на получение расписания</param>
        /// <returns></returns>
        public Task<List<ScheduleEntity>?> GetByUserdataIdAsync(ScheduleRequest scheduleRequest);

        /// <summary>
        /// Создаёт новое расписание в базе данных
        /// </summary>
        /// <param name="schedule">Создаваемое расписание</param>
        /// <returns></returns>
        public Task CreateAsync(ScheduleEntity schedule);

        /// <summary>
        /// Обновляет расписание в базе данных
        /// </summary>
        /// <param name="schedule">Обновляемое расписание</param>
        /// <returns></returns>
        public Task UpdateAsync(ScheduleEntity schedule);

        /// <summary>
        /// Удаляет расписание из базы данных
        /// </summary>
        /// <param name="scheduleId">ID удаляемого расписания</param>
        /// <returns></returns>
        public Task DeleteAsync(string scheduleId);

        /// <summary>
        /// Удаляет несколько расписаний из базы данных
        /// </summary>
        /// <param name="scheduleIds">Список ID удаляемых расписаний</param>
        /// <returns></returns>
        public Task DeleteAsync(List<string> scheduleIds);
    }
}
