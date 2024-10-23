using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Domain.Requests.Schedule;

namespace WorkyOne.AppServices.Interfaces.Services.Schedule.Common
{
    /// <summary>
    /// Интерфейс сервиса по работе с расписанием
    /// </summary>
    public interface IScheduleService
    {
        /// <summary>
        /// Возвращает расписание из базы данных
        /// </summary>
        /// <param name="scheduleId">Идентификатор расписания</param>
        /// <param name="cancellation">Токен отмены задания</param>
        public Task<ScheduleDto?> GetAsync(
            string scheduleId,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Возвращает из базы данных расписания, относящиеся к определённому пользователю
        /// </summary>
        /// <param name="userDataId">Идентификатор пользовательских данных</param>
        /// <param name="cancellation">Токен отмены задания</param>
        /// <returns></returns>
        public Task<List<ScheduleDto>> GetByUserDataAsync(
            string userDataId,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Возвращает множестно расписаний из базы данных
        /// </summary>
        /// <param name="pageIndex">Номер страницы</param>
        /// <param name="amount">Количество на странице</param>
        /// <param name="includeFullData">Включать ли все данные в запрос</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<List<ScheduleDto>> GetManyAsync(
            int pageIndex,
            int amount,
            bool includeFullData = false,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Создаёт расписание в базе данных. Возвращает идентификатор созданного расписания в случае успеха
        /// </summary>
        /// <param name="scheduleName">Название расписания</param>
        /// <param name="userDataId">ID пользовательских данных, для которых создаётся расписание</param>
        /// <param name="cancellation">Токен отмены задания</param>

        public Task<string?> CreateScheduleAsync(
            string scheduleName,
            string userDataId,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Удаляет из базы данных расписания с указанными ID
        /// </summary>
        /// <param name="schedulesIds">Список ID удаляемых расписаний</param>
        /// <param name="cancellation">Токен отмены задания</param>

        public Task<bool> DeleteSchedulesAsync(
            List<string> schedulesIds,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Обновляет расписание в базе данных
        /// </summary>
        /// <param name="scheduleDto">DTO обновляемого расписания</param>
        /// <param name="cancellation">Токен отмены задания</param>

        public Task<bool> UpdateScheduleAsync(
            ScheduleDto scheduleDto,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Создаёт и сохраняет в базу данных рабочий график согласно заданному расписанию
        /// </summary>
        /// <param name="scheduleId">Идентификатор расписания, согласно которому создаётся график</param>
        /// <param name="startDate">Дата, с которой начинается расчёт графика</param>
        /// <param name="endDate">Дата, которой оканчивается расчёт графика</param>
        /// <param name="cancellation">Токен отмены задания</param>

        public Task<List<DailyInfoDto>> GenerateDailyAsync(
            string scheduleId,
            DateOnly startDate,
            DateOnly endDate,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Удаляет из базы данных рассчитанный рабочий график для указанного расписания
        /// </summary>
        /// <param name="scheduleId">Идентификатор расписания, для которого удаляется график</param>
        /// <param name="cancellation">Токен отмены задания</param>

        public Task<bool> ClearDailyAsync(
            string scheduleId,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Возвращает рабочий график для указанного расписания
        /// </summary>
        /// <param name="scheduleId">Идентификатор расписания</param>
        /// <param name="cancellation">Токен отмены задания</param>

        public Task<List<DailyInfoDto>> GetDailyAsync(
            string scheduleId,
            DateOnly startDate,
            DateOnly endDate,
            CancellationToken cancellation = default
        );
    }
}
