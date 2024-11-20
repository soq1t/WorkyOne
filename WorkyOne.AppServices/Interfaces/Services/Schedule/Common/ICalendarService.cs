using System.Globalization;
using WorkyOne.Contracts.DTOs.Abstractions;
using WorkyOne.Contracts.Services.Common;
using WorkyOne.Contracts.Services.Requests;

namespace WorkyOne.AppServices.Interfaces.Services.Schedule.Common
{
    /// <summary>
    /// Интерфейс сервиса для работы с календарём
    /// </summary>
    public interface ICalendarService
    {
        /// <summary>
        /// Возвращает информацию о календаре
        /// </summary>
        /// <param name="request">Запрос на получение данных о календаре</param>
        public CalendarInfo GetCalendarInfo(CalendarInfoRequest request);

        /// <summary>
        /// Возвращает информацию о календаре на текущий день
        /// </summary>
        public CalendarInfo GetNowCalendarInfo();

        /// <summary>
        /// Возвращает список дней недели
        /// </summary>
        /// <param name="cultureInfo">Информация культуры, для которой необходимо получить список дней недели</param>
        public List<string> GetWeekdaysNames(CultureInfo? cultureInfo = null);

        /// <summary>
        /// Возвращает информацию о рабочем графике на месяц
        /// </summary>
        /// <param name="calendarInfoRequest">Запрос на получение данных о календаре</param>
        /// <param name="scheduleId">Идентификатор расписания, для которой запрашивается график</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<MonthGraphicInfo> GetMonthGraphicAsync(
            CalendarInfoRequest calendarInfoRequest,
            string scheduleId,
            CancellationToken cancellation = default
        );

        /// <summary>
        /// Возвращает информацию о рабочем графике на месяц
        /// </summary>
        /// <param name="calendarInfo">Информация о календаре</param>
        /// <param name="scheduleId">Идентификатор расписания, для которой запрашивается график</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        public Task<MonthGraphicInfo> GetMonthGraphicAsync(
            CalendarInfo calendarInfo,
            string scheduleId,
            CancellationToken cancellation = default
        );
    }
}
