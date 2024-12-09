using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Mvc;
using WorkyOne.AppServices.Interfaces.Services.Auth;
using WorkyOne.Contracts.Services.Requests;
using WorkyOne.MVC.Models.Authentification;

namespace WorkyOne.MVC.Controllers.Account
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        [Route("signin")]
        public IActionResult SignIn(CancellationToken cancellation = default)
        {
            if (_authService.IsAuthenticated())
            {
                return LocalRedirect("/");
            }
            else
            {
                return View(new LogInViewModel() { });
            }
        }

        [ValidateReCaptcha]
        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> SignIn(
            [FromForm] LogInViewModel model,
            CancellationToken cancellation = default
        )
        {
            if (_authService.IsAuthenticated())
            {
                return LocalRedirect("/");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authService.LogInAsync(
                new LogInRequest
                {
                    Username = model.Username,
                    Password = model.Password,
                    CreateSession = model.RememberMe
                },
                cancellation
            );

            if (result.Succeeded)
            {
                return LocalRedirect("/");
            }
            else
            {
                ModelState.AddModelError(
                    "",
                    "Неверный логин или пароль (либо ваш аккаунт не активирован)"
                );
                return View(model);
            }
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            if (_authService.IsAuthenticated())
            {
                return LocalRedirect("/");
            }

            return View(new RegistrationViewModel());
        }

        [HttpPost]
        [ValidateReCaptcha]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync(
            [FromForm] RegistrationViewModel model,
            CancellationToken cancellation = default
        )
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _authService.RegisterAsync(
                new RegistrationRequest()
                {
                    Username = model.Username,
                    FirstName = model.FirstName,
                    Password = model.Password
                },
                cancellation
            );

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> SignOut(CancellationToken cancellation = default)
        {
            if (_authService.IsAuthenticated())
            {
                await _authService.LogOutAsync(cancellation);
            }

            return LocalRedirect("/");
        }
    }
}
