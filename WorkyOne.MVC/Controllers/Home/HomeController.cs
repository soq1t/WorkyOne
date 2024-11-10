using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services;
using WorkyOne.AppServices.Interfaces.Services.Auth;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services.Users;
using WorkyOne.Contracts.Services.GetRequests.Schedule.Common;
using WorkyOne.Contracts.Services.GetRequests.Users;
using WorkyOne.MVC.Models;
using WorkyOne.MVC.Models.Common;
using WorkyOne.MVC.Models.Schedule;

namespace WorkyOne.MVC.Controllers.Home
{
    public class HomeController : Controller
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly IAuthService _authService;
        private readonly IUsersService _userService;
        private readonly IWorkGraphicService _workGraphicService;
        private readonly ILogger<HomeController> _logger;
        private readonly ICalendarService _calendarService;

        public HomeController(
            ILogger<HomeController> logger,
            IDateTimeService dateTimeService,
            IAuthService authService,
            IUsersService userService,
            IWorkGraphicService workGraphicService,
            ICalendarService calendarService
        )
        {
            _logger = logger;
            _dateTimeService = dateTimeService;
            _authService = authService;
            _userService = userService;
            _workGraphicService = workGraphicService;
            _calendarService = calendarService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellation = default)
        {
            var calendarInfo = _calendarService.GetNowCalendarInfo();

            var model = new HomeViewModel
            {
                CalendarViewModel = new CalendarViewModel
                {
                    Year = calendarInfo.Year,
                    Month = calendarInfo.MonthNumber,
                    MonthName = calendarInfo.MonthName,
                }
            };

            if (_authService.IsAuthenticated())
            {
                var userInfo = await _userService.GetUserInfoAsync(
                    new UserInfoRequest
                    {
                        IncludeSchedules = true,
                        UserName = HttpContext.User.Identity.Name
                    },
                    cancellation
                );

                var schedule = userInfo?.Schedules.FirstOrDefault();

                if (schedule != null)
                {
                    model.CalendarViewModel.ScheduleDto = schedule;
                    model.CalendarViewModel.WorkGraphic = await _workGraphicService.GetGraphicAsync(
                        new PaginatedWorkGraphicRequest
                        {
                            PageIndex = 1,
                            Amount = calendarInfo.DaysAmount,
                            ScheduleId = schedule.Id,
                            StartDate = calendarInfo.Start,
                            EndDate = calendarInfo.End,
                        },
                        cancellation
                    );
                }
            }

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
