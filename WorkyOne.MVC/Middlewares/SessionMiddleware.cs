using System.Diagnostics;
using WorkyOne.AppServices.Interfaces.Services.Auth;

namespace WorkyOne.MVC.Middlewares
{
    public class SessionMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IJwtService jwtService,
            ISessionService sessionService
        )
        {
            var jwtToken = jwtService.GetFromCookies();

            if (jwtToken != null)
            {
                jwtService.WriteToHeaders(jwtToken);
            }
            else
            {
                var sessionToken = sessionService.GetTokenFromCookies();

                if (sessionToken != null && await sessionService.VerifyTokenAsync(sessionToken))
                {
                    sessionToken = await sessionService.RefreshTokenAsync(sessionToken);
                    sessionService.WriteTokenToCookies(sessionToken);

                    var session = await sessionService.GetSessionByTokenAsync(sessionToken);
                    jwtToken = await jwtService.GenerateJwtTokenAsync(session.UserId);

                    if (jwtToken != null)
                    {
                        jwtService.WriteToCookies(jwtToken);
                        jwtService.WriteToHeaders(jwtToken);
                    }
                }
            }

            await _next(context);
        }
    }
}
