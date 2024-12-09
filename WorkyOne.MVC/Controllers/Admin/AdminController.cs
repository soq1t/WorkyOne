using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WorkyOne.AppServices.Interfaces.Services.Users;
using WorkyOne.Contracts.Options.Common;
using WorkyOne.Contracts.Services.GetRequests.Common;
using WorkyOne.Contracts.Services.Requests;
using WorkyOne.MVC.Models.Admin;

namespace WorkyOne.MVC.Controllers.Admin
{
    [Authorize(Roles = "Admin,God")]
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IOptions<PaginationOptions> _paginationOptions;

        public AdminController(
            IUsersService usersService,
            IOptions<PaginationOptions> paginationOptions
        )
        {
            _usersService = usersService;
            _paginationOptions = paginationOptions;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("users")]
        public async Task<IActionResult> UsersAsync(
            UsersRequest? request,
            CancellationToken cancellation = default
        )
        {
            if (request == null)
            {
                request = new UsersRequest();
            }

            var response = await _usersService.GetUsersAsync(
                new PaginatedRequest
                {
                    Amount = _paginationOptions.Value.PageSize,
                    PageIndex = request.Page
                },
                request.Filter,
                cancellation
            );

            var model = new UsersViewModel() { Users = response.Value, Filter = request.Filter };

            model.Pagination.PageIndex = response.PageIndex;
            model.Pagination.PagesAmount = response.PageAmount;

            return View(model);
        }

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
    }
}
