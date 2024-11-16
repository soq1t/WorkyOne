using System.Globalization;
using WorkyOne.AppServices.Interfaces.Services;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.Contracts.Services.Common;
using WorkyOne.Contracts.Services.Requests;

namespace WorkyOne.AppServices.Services.Schedule.Common
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

        public CalendarInfo GetCalendarInfo(CalendarInfoRequest request)
        {
            var calendarInfo = new CalendarInfo
            {
                Year = request.Year,
                MonthNumber = request.Month,
                MonthName = GetMonthName(request.Month)
            };

            calendarInfo.Start = GetStartDate(request.Year, request.Month);
            calendarInfo.End = GetEndDate(request.Year, request.Month);
            calendarInfo.DaysAmount = GetDaysAmount(calendarInfo.Start, calendarInfo.End);

            if (calendarInfo.DaysAmount / 7 < 6)
            {
                calendarInfo.DaysAmount += 7;
                calendarInfo.End = calendarInfo.End.AddDays(7);
            }
            return calendarInfo;
        }

        public CalendarInfo GetNowCalendarInfo()
        {
            var now = _dateTimeService.GetNow();

            var request = new CalendarInfoRequest { Year = now.Year, Month = now.Month };

            return GetCalendarInfo(request);
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

        public List<string> GetWeekdaysNames(CultureInfo? cultureInfo = null)
        {
            cultureInfo ??= CultureInfo.CurrentCulture;

            var dateTimeFormat = cultureInfo.DateTimeFormat;

            return ReorderDays(dateTimeFormat.AbbreviatedDayNames);
        }

        private static List<string> ReorderDays(string[] days)
        {
            var result = new List<string>(7);

            for (int i = 1; i <= 6; i++)
            {
                result.Add(days[i]);
            }

            result.Add(days[0]);

            return result;
        }
    }
}
