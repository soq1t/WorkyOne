using System.Globalization;
using AutoMapper;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Stores;
using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Contracts.Services.Common;
using WorkyOne.Contracts.Services.GetRequests.Schedule.Common;
using WorkyOne.Contracts.Services.Requests;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Requests.Schedule.Common;
using WorkyOne.Domain.Specifications.Common;

namespace WorkyOne.AppServices.Services.Schedule.Common
{
    /// <summary>
    /// Сервис для работы с календарём
    /// </summary>
    public class CalendarService : ICalendarService
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly IWorkGraphicService _workGraphicService;
        private readonly ISchedulesRepository _schedulesRepository;
        private readonly IMapper _mapper;

        private readonly IAccessFiltersStore _accessFiltersStore;

        public CalendarService(
            IDateTimeService dateTimeService,
            ISchedulesRepository schedulesRepository,
            IAccessFiltersStore accessFiltersStore,
            IMapper mapper,
            IWorkGraphicService workGraphicService
        )
        {
            _dateTimeService = dateTimeService;
            _schedulesRepository = schedulesRepository;
            _accessFiltersStore = accessFiltersStore;
            _mapper = mapper;
            _workGraphicService = workGraphicService;
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

        public Task<MonthGraphicInfo> GetMonthGraphicAsync(
            CalendarInfoRequest calendarInfoRequest,
            string scheduleId,
            CancellationToken cancellation = default
        )
        {
            var calendarInfo = GetCalendarInfo(calendarInfoRequest);

            return GetMonthGraphicAsync(calendarInfo, scheduleId, cancellation);
        }

        public async Task<MonthGraphicInfo> GetMonthGraphicAsync(
            CalendarInfo calendarInfo,
            string scheduleId,
            CancellationToken cancellation = default
        )
        {
            var filter = new EntityIdFilter<ScheduleEntity>(scheduleId).And(
                _accessFiltersStore.GetFilter<ScheduleEntity>()
            );

            var monthGraphicInfo = new MonthGraphicInfo();

            monthGraphicInfo.CalendarInfo = calendarInfo;

            var schedule = await _schedulesRepository.GetAsync(
                new ScheduleRequest(filter) { IncludeShifts = true, IncludeTemplate = true },
                cancellation
            );

            if (schedule == null)
            {
                return monthGraphicInfo;
            }

            monthGraphicInfo.Schedule = _mapper.Map<ScheduleDto>(schedule);

            var graphic = await _workGraphicService.GetGraphicAsync(
                new PaginatedWorkGraphicRequest
                {
                    ScheduleId = scheduleId,
                    PageIndex = 1,
                    Amount = calendarInfo.DaysAmount,
                    StartDate = calendarInfo.Start,
                    EndDate = calendarInfo.End
                }
            );

            for (var date = calendarInfo.Start; date <= calendarInfo.End; date = date.AddDays(1))
            {
                var dailyInfo = graphic.FirstOrDefault(d => d.Date == date);

                if (dailyInfo == null)
                {
                    dailyInfo = new DailyInfoDto
                    {
                        IsBusyDay = false,
                        Date = date,
                        Name = "Свободный день",
                        ColorCode = "#93cfd7"
                    };
                }

                monthGraphicInfo.Graphic.Add(dailyInfo);

                if (!monthGraphicInfo.Legend.ContainsKey(dailyInfo.Name))
                {
                    monthGraphicInfo.Legend.Add(dailyInfo.Name, dailyInfo.ColorCode);
                }
            }

            monthGraphicInfo.Legend = monthGraphicInfo.Legend.OrderBy(x => x.Key).ToDictionary();

            return monthGraphicInfo;
        }
    }
}
