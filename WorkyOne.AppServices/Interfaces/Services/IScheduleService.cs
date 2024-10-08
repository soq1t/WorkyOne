using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Contracts.DTOs.Schedule;

namespace WorkyOne.AppServices.Interfaces.Services
{
    /// <summary>
    /// Интерфейс сервиса по работе со сменами
    /// </summary>
    public interface IScheduleService
    {
        /// <summary>
        /// Создаёт расписание в базе данных
        /// </summary>
        /// <param name="scheduleName">Название расписания</param>
        /// <param name="userDataId">ID пользовательских данных, для которых создаётся расписание</param>
        /// <returns></returns>
        public Task CreateScheduleAsync(string scheduleName, string userDataId);

        /// <summary>
        /// Удаляет расписания с указанными ID
        /// </summary>
        /// <param name="schedulesIds">Список ID удаляемых расписаний</param>
        /// <returns></returns>
        public Task DeleteSchedulesAsync(List<string> schedulesIds);

        /// <summary>
        /// Обновляет расписание в базе данных
        /// </summary>
        /// <param name="scheduleDto">DTO обновляемого расписания</param>
        /// <returns></returns>
        public Task UpdateScheduleAsync(ScheduleDto scheduleDto);
    }
}
