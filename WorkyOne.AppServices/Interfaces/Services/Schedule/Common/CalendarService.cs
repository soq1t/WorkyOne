using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.Contracts.Services.Common;

namespace WorkyOne.AppServices.Interfaces.Services.Schedule.Common
{
    /// <summary>
    /// Сервис для работы с календарём
    /// </summary>
    public class CalendarService : ICalendarService
    {
        public CalendarInfo GetCalendarInfo(int year, int month)
        {
            var calendarInfo = new CalendarInfo();

            calendarInfo.Start = GetStartDate(year, month);
            calendarInfo.End = GetEndDate(year, month);

            return calendarInfo;
        }

        private DateOnly GetStartDate(int year, int month)
        {
            var firstDay = new DateOnly(year, month, 1);

            var dayOfWeekNumber = (int)firstDay.DayOfWeek;

            if (dayOfWeekNumber == 1)
            {
                return firstDay;
            }
            else if (dayOfWeekNumber == 0)
            {
                return firstDay.AddDays(-6);
            }
            else
            {
                var amount = dayOfWeekNumber - 1;
                return firstDay.AddDays(-amount);
            }
        }

        private DateOnly GetEndDate(int year, int month)
        {
            var lastDayNumber = DateTime.DaysInMonth(year, month);

            var lastDay = new DateOnly(year, month, lastDayNumber);

            var dayOfWeekNumber = (int)lastDay.DayOfWeek;

            if (dayOfWeekNumber == 0)
            {
                return lastDay;
            }
            else
            {
                return lastDay.AddDays(7 - dayOfWeekNumber);
            }
        }
    }
}
