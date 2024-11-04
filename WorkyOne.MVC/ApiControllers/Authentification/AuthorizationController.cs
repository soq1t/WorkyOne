using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WorkyOne.AppServices.Interfaces.Services.Auth;
using WorkyOne.Contracts.Options.Auth;
using WorkyOne.Contracts.Services.Requests;
using WorkyOne.MVC.ViewModels.Authentification;

namespace WorkyOne.MVC.ApiControllers.Authentification
{
    [ApiController]
    [Route("api/auth")]
    public class AuthorizationController : Controller
    {
        private readonly IJwtService _jwtService;
        private readonly IAuthService _authService;

        public AuthorizationController(IJwtService jwtService, IAuthService authService)
        {
            _jwtService = jwtService;
            _authService = authService;
        }

        [HttpPost]
        [Route("jwt")]
        public async Task<IActionResult> GetJwtToken([FromBody] LogInViewModel model)
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

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> SignInAsync(
            [FromBody] LogInViewModel model,
            CancellationToken cancellation = default
        )
        {
            var request = new LogInRequest
            {
                Username = model.Username,
                Password = model.Password,
                CreateSession = model.RememberMe
            };

            var result = await _authService.LogInAsync(request, cancellation);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("signout")]
        public async Task<IActionResult> SignOutAsync(CancellationToken cancellation = default)
        {
            await _authService.LogOutAsync(cancellation);

            return Ok();
        }
    }
}
