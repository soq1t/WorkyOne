using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Domain.Entities.Schedule.Common;

namespace WorkyOne.AppServices.Interfaces.Services
{
    /// <summary>
    /// Интерфейс сервиса по работе с расписанием
    /// </summary>
    public interface IScheduleService
    {
        /// <summary>
        /// Создаёт расписание в базе данных. Возвращает идентификатор созданного расписания в случае успеха
        /// </summary>
        /// <param name="scheduleName">Название расписания</param>
        /// <param name="userDataId">ID пользовательских данных, для которых создаётся расписание</param>
        /// <returns></returns>
        public Task<string?> CreateScheduleAsync(string scheduleName, string userDataId);

        /// <summary>
        /// Удаляет из базы данных расписания с указанными ID
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

        /// <summary>
        /// Создаёт и сохраняет в базу данных рабочий график согласно заданному расписанию
        /// </summary>
        /// <param name="scheduleId">Идентификатор расписания, согласно которому создаётся график</param>
        /// <param name="startDate">Дата, с которой начинается расчёт графика</param>
        /// <param name="endDate">Дата, которой оканчивается расчёт графика</param>
        /// <returns></returns>
        public Task GenerateDailyAsync(string scheduleId, DateOnly startDate, DateOnly endDate);

        /// <summary>
        /// Удаляет из базы данных рассчитанный рабочий график для указанного расписания
        /// </summary>
        /// <param name="scheduleId">Идентификатор расписания, для которого удаляется график</param>
        /// <returns></returns>
        public Task ClearDailyAsync(string scheduleId);

        /// <summary>
        /// Возвращает рабочий график для указанного расписания
        /// </summary>
        /// <param name="scheduleId">Идентификатор расписания</param>
        /// <returns></returns>
        public Task<ICollection<DailyInfoDto>> GetDailyAsync(string scheduleId);
    }
}
