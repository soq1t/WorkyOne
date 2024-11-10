using WorkyOne.Contracts.Services.Common;

namespace WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common
{
    /// <summary>
    /// Интерфейс сервиса для работы с календарём
    /// </summary>
    public interface ICalendarService
    {
        /// <summary>
        /// Возвращает информацию о календаре
        /// </summary>
        /// <param name="year">Год</param>
        /// <param name="month">Порядковый номер месяца</param>
        public CalendarInfo GetCalendarInfo(int year, int month);

        /// <summary>
        /// Возвращает информацию о календаре на текущий день
        /// </summary>
        public CalendarInfo GetNowCalendarInfo();
    }
}
