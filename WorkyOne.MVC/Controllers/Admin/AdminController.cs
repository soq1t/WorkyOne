using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using WorkyOne.AppServices.Interfaces.Services.Auth;
using WorkyOne.AppServices.Interfaces.Services.Users;
using WorkyOne.Contracts.Options.Common;
using WorkyOne.Contracts.Services.GetRequests.Common;
using WorkyOne.Contracts.Services.GetRequests.Users;
using WorkyOne.MVC.Models.Admin;

namespace WorkyOne.MVC.Controllers.Admin
{
    [Authorize(Roles = "Admin,God")]
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IAuthService _authService;
        private readonly IRolesService _rolesService;
        private readonly IOptions<PaginationOptions> _paginationOptions;

        public AdminController(
            IUsersService usersService,
            IOptions<PaginationOptions> paginationOptions,
            IAuthService authService,
            IRolesService rolesService
        )
        {
            _usersService = usersService;
            _paginationOptions = paginationOptions;
            _authService = authService;
            _rolesService = rolesService;
        }

        /// <summary>
        /// Главная страница администрирования приложения
        /// </summary>
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Возвращает страницу управления пользователями
        /// </summary>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpGet]
        [Route("users")]
        public async Task<IActionResult> UsersAsync(CancellationToken cancellation = default)
        {
            var response = await _usersService.GetUsersAsync(
                new PaginatedRequest { Amount = _paginationOptions.Value.PageSize, PageIndex = 1 },
                new UserFilter(),
                cancellation
            );

            var model = new UsersViewModel() { Users = response.Value, Filter = new UserFilter() };

            model.Pagination.PageIndex = response.PageIndex;
            model.Pagination.PagesAmount = response.PageAmount;

            return View(model);
        }

        /// <summary>
        /// Возвращает страницу с пользователями на основе модели данных
        /// </summary>
        /// <param name="model">Модель с данными для страницы пользователей</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpPost]
        [Route("users")]
        public async Task<IActionResult> UsersAsync(
            [FromForm] UsersViewModel model,
            CancellationToken cancellation = default
        )
        {
            var response = await _usersService.GetUsersAsync(
                new PaginatedRequest
                {
                    Amount = _paginationOptions.Value.PageSize,
                    PageIndex = model.Pagination.PageIndex
                },
                model.Filter,
                cancellation
            );

            model = new UsersViewModel() { Users = response.Value, Filter = model.Filter };

            model.Pagination.PageIndex = response.PageIndex;
            model.Pagination.PagesAmount = response.PageAmount;

            return View(model);
        }

        /// <summary>
        /// Возвращает страницу редактирования пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpGet]
        [Route("users/{id}")]
        public async Task<IActionResult> UserAsync(
            [FromRoute] string id,
            CancellationToken cancellation = default
        )
        {
            var model = new UserViewModel();
            if (_authService.IsUserInRoles("God"))
            {
                model.Roles = await _rolesService.GetRolesAsync(cancellation);
            }

            var userInfo = await _usersService.GetUserInfoAsync(
                new UserInfoRequest() { UserId = id, IncludeSchedules = true },
                cancellation
            );

            if (userInfo == null)
            {
                return BadRequest();
            }

            model.User = userInfo;
            model.SchedulesAmount = userInfo.Schedules.Count;

            return PartialView("/Views/Admin/Users/_UserPartial.cshtml", model);
        }

        [HttpPost]
        [Route("users/{id}")]
        public async Task<IActionResult> UpdateUserAsync(
            [FromForm] UserViewModel model,
            CancellationToken cancellation = default
        )
        {
            ModelState["User.Schedules"].ValidationState = ModelValidationState.Valid;
            ModelState["User.UserDataId"].ValidationState = ModelValidationState.Valid;
            ModelState["User.Email"].ValidationState = ModelValidationState.Valid;

            if (!ModelState.IsValid)
            {
                model.Roles = await _rolesService.GetRolesAsync(cancellation);

                return PartialView("/Views/Admin/Users/_UserPartial.cshtml", model);
            }
            else
            {
                await _usersService.UpdateUserAsync(model.User, cancellation);
                return Ok();
            }
        }

        /// <summary>
        /// Производит активацию пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <param name="cancellation">Токен отмены задачи</param>
        [HttpGet]
        [Route("users/{id}/activate")]
        public async Task<IActionResult> ActivateUserAsync(
            [FromRoute] string id,
            CancellationToken cancellation = default
        )
        {
            await _usersService.ActivateUserAsync(id, cancellation);

            return Ok();
        }
    }
}
