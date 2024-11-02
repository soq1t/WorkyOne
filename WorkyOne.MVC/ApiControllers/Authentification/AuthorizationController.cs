using Microsoft.AspNetCore.Mvc;
using WorkyOne.AppServices.Interfaces.Services.Users;
using WorkyOne.MVC.ViewModels.Api.Authentification;

namespace WorkyOne.MVC.ApiControllers.Authentification
{
    [ApiController]
    [Route("api/auth")]
    public class AuthorizationController : Controller
    {
        private readonly IJwtService _jwtService;

        public AuthorizationController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost]
        [Route("jwt")]
        public async Task<IActionResult> GetJwtToken([FromBody] UserViewModel model)
        {
            var token = await _jwtService.GenerateJwtTokenAsync(model.Username, model.Password);

            if (token == null)
            {
                return BadRequest("Wrong username or password");
            }
            else
            {
                return Ok(token);
            }
        }
    }
}
