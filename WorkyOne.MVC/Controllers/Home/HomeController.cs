using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WorkyOne.AppServices.Interfaces.Services;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services.Users;
using WorkyOne.Contracts.Services.GetRequests.Users;
using WorkyOne.MVC.Models;
using WorkyOne.MVC.Models.Common;
using WorkyOne.MVC.Models.Schedule;

namespace WorkyOne.MVC.Controllers.Home
{
    public class HomeController : Controller
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly IUsersService _userService;
        private readonly ICalendarService _calendarService;

        public HomeController(
            IDateTimeService dateTimeService,
            IUsersService userService,
            ICalendarService calendarService
        )
        {
            _dateTimeService = dateTimeService;
            _userService = userService;
            _calendarService = calendarService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellation = default)
        {
            var calendarInfo = _calendarService.GetNowCalendarInfo();

            var userInfo = await _userService.GetUserInfoAsync(
                new UserInfoRequest { IsCurrentUserRequired = true, IncludeSchedules = true }
            );

            var schedules = userInfo?.Schedules;

            if (schedules != null)
            {
                schedules = schedules
                    .OrderByDescending(x => x.Id == userInfo.FavoriteScheduleId)
                    .ToList();
            }

            var monthGraphic = await _calendarService.GetMonthGraphicAsync(
                calendarInfo,
                schedules?.FirstOrDefault()?.Id ?? string.Empty,
                cancellation
            );

            var model = new HomeViewModel
            {
                CalendarViewModel = new CalendarViewModel
                {
                    Info = calendarInfo,
                    Legend = monthGraphic.Legend,
                    ScheduleDto = monthGraphic.Schedule,
                    WorkGraphic = monthGraphic.Graphic
                },
                Schedules = schedules
            };

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                }
            );
        }
    }
}
