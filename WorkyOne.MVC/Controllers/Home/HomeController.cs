using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkyOne.AppServices.Interfaces.Services;
using WorkyOne.AppServices.Interfaces.Services.Auth;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services.Users;
using WorkyOne.Contracts.Services.GetRequests.Schedule.Common;
using WorkyOne.Contracts.Services.GetRequests.Users;
using WorkyOne.Contracts.Services.Requests;
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
            var now = _dateTimeService.GetNow();
            var model = new HomeViewModel { Year = now.Year, Month = now.Month };

            return View(model);
        }

        [HttpPost]
        [Route("calendar")]
        public async Task<IActionResult> GetCalendarAsync(
            [FromBody] CalendarInfoRequest request,
            CancellationToken cancellation = default
        )
        {
            var calendarInfo = _calendarService.GetCalendarInfo(request);

            var model = new CalendarViewModel { Info = calendarInfo };

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
                model.ScheduleDto = schedule;
                model.WorkGraphic = await _workGraphicService.GetGraphicAsync(
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

            return PartialView("_CalendarPartial", model);
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
