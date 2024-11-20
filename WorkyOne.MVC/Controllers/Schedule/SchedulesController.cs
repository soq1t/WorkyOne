using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WorkyOne.AppServices.Interfaces.Services.Schedule.Common;
using WorkyOne.AppServices.Interfaces.Services.Users;
using WorkyOne.Contracts.DTOs.Schedule.Common;
using WorkyOne.Contracts.Options.Common;
using WorkyOne.Contracts.Services.GetRequests.Schedule.Common;
using WorkyOne.Contracts.Services.GetRequests.Users;
using WorkyOne.MVC.Models.Schedule;

namespace WorkyOne.MVC.Controllers.Schedule
{
    /// <summary>
    /// Контроллер по работе с расписаниями пользователя
    /// </summary>
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

        /// <summary>
        /// Главная страница с расписаниями
        /// </summary>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index(CancellationToken cancellation = default)
        {
            var userInfo = await _usersService.GetUserInfoAsync(
                new UserInfoRequest { IsCurrentUserRequired = true },
                cancellation
            );

            if (userInfo == null)
            {
                return NotFound();
            }

            var schedules = await _scheduleService.GetByUserAsync(
                userInfo.Id,
                new PaginatedScheduleRequest
                {
                    PageIndex = 1,
                    IncludeFullData = false,
                    Amount = _paginationOptions.Value.PageSize
                },
                cancellation
            );

            var model = new SchedulesViewModel
            {
                Schedules = schedules,
                FavoriteScheduleId = userInfo.FavoriteScheduleId
            };

            return View(model);
        }

        /// <summary>
        /// Возвращает страницу с указанным расписанием
        /// </summary>
        /// <param name="id">Идентификатор расписания</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetScheduleAsync(
            [FromRoute] string id,
            CancellationToken cancellation = default
        )
        {
            var schedule = await _scheduleService.GetAsync(id, cancellation);

            if (schedule != null)
            {
                return View("Schedule", schedule);
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Возвращает страницу для создания расписания
        /// </summary>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpGet]
        [Route("create")]
        public IActionResult CreateScheduleAsync(CancellationToken cancellation = default)
        {
            return View("Schedule", new ScheduleDto() { Name = "Новое расписание" });
        }

        /// <summary>
        /// Меняет "любимое" расписание
        /// </summary>
        /// <param name="id">Идентификатор расписания</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpPut]
        [Route("{id}/favorite")]
        public async Task<IActionResult> ChangeFavoriteAsync(
            [FromRoute] string id,
            CancellationToken cancellation = default
        )
        {
            var userInfo = await _usersService.GetUserInfoAsync(
                new UserInfoRequest { IsCurrentUserRequired = true },
                cancellation
            );

            if (userInfo == null)
            {
                return Unauthorized();
            }

            var result = await _usersService.SetFavoriteScheduleAsync(
                userInfo.UserDataId,
                id,
                cancellation
            );

            if (result.IsSucceed)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
