using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.Contracts.Services.Common;

namespace WorkyOne.AppServices.Interfaces.Services.Schedule.Common
{
    /// <summary>
    /// Сервис для работы с календарём
    /// </summary>
    public class CalendarService : ICalendarService
    {
        private readonly IDateTimeService _dateTimeService;

        public CalendarService(IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
        }

        public CalendarInfo GetCalendarInfo(int year, int month)
        {
            var calendarInfo = new CalendarInfo
            {
                Year = year,
                MonthNumber = month,
                MonthName = GetMonthName(month)
            };

            calendarInfo.Start = GetStartDate(year, month);
            calendarInfo.End = GetEndDate(year, month);
            calendarInfo.DaysAmount = GetDaysAmount(calendarInfo.Start, calendarInfo.End);
            return calendarInfo;
        }

        public CalendarInfo GetNowCalendarInfo()
        {
            var now = _dateTimeService.GetNow();

            return GetCalendarInfo(now.Year, now.Month);
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

        private string GetMonthName(int month)
        {
            var date = new DateTime(2000, month, 1);

            return char.ToUpper(date.ToString("MMMM")[0]) + date.ToString("MMMM").Substring(1);
        }

        private int GetDaysAmount(DateOnly start, DateOnly end)
        {
            return end.DayNumber - start.DayNumber + 1;
        }
    }
}
