using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services.Users;
using WorkyOne.Contracts.Options.Common;
using WorkyOne.Contracts.Services.GetRequests.Schedule.Common;
using WorkyOne.Contracts.Services.GetRequests.Users;
using WorkyOne.MVC.Models.Schedule;

namespace WorkyOne.MVC.Controllers.Schedule
{
    [Authorize]
    [Route("schedules")]
    public class SchedulesController : Controller
    {
        private readonly IScheduleService _scheduleService;
        private readonly IUsersService _usersService;
        private readonly IOptions<PaginationOptions> _paginationOptions;

        public SchedulesController(
            IScheduleService scheduleService,
            IUsersService usersService,
            IOptions<PaginationOptions> paginationOptions
        )
        {
            _scheduleService = scheduleService;
            _usersService = usersService;
            _paginationOptions = paginationOptions;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index(CancellationToken cancellation = default)
        {
            var user = await _usersService.GetUserInfoAsync(
                new UserInfoRequest { IsCurrentUserRequired = true },
                cancellation
            );

            if (user == null)
            {
                return NotFound();
            }

            var schedules = await _scheduleService.GetByUserAsync(
                user.Id,
                new PaginatedScheduleRequest
                {
                    PageIndex = 1,
                    IncludeFullData = false,
                    Amount = _paginationOptions.Value.PageSize
                },
                cancellation
            );

            var model = new SchedulesViewModel { Schedules = schedules };

            return View(model);
        }
    }
}
