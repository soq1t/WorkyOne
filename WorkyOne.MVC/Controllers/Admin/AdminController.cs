using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkyOne.AppServices.Interfaces.Services.Users;
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

        public AdminController(IUsersService usersService)
        {
            _usersService = usersService;
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
                new PaginatedRequest { Amount = 4, PageIndex = request.Page },
                request.Filter,
                cancellation
            );

            var model = new UsersViewModel() { Users = response.Value, };

            model.Pagination.PageIndex = response.PageIndex;
            model.Pagination.PagesAmount = response.PageAmount;

            return View(model);
        }
    }
}
