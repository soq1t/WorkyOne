using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WorkyOne.MVC.Controllers.Admin
{
    [Authorize(Roles = "Admin,God")]
    [Route("admin")]
    public class AdminController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
