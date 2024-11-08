using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WorkyOne.AppServices.Interfaces.Services;
using WorkyOne.AppServices.Interfaces.Services.Auth;
using WorkyOne.AppServices.Interfaces.Services.Users;
using WorkyOne.MVC.Models;
using WorkyOne.MVC.Models.Common;
using WorkyOne.MVC.Models.Schedule;

namespace WorkyOne.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly IAuthService _authService;
        private readonly IUsersService _userService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ILogger<HomeController> logger,
            IDateTimeService dateTimeService,
            IAuthService authService,
            IUsersService userService
        )
        {
            _logger = logger;
            _dateTimeService = dateTimeService;
            _authService = authService;
            _userService = userService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellation = default)
        {
            var now = _dateTimeService.GetNow();
            var year = now.Year;
            var month = now.Month;
            var monthName =
                char.ToUpper(now.ToString("MMMM")[0]) + now.ToString("MMMM").Substring(1);

            var model = new HomeViewModel
            {
                CalendarViewModel = new CalendarViewModel
                {
                    Year = year,
                    Month = month,
                    MonthName = monthName,
                }
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
