namespace WorkyOne.MVC.Middlewares
{
    /// <summary>
    /// Миддлвэйр, перенаправляющий на страницу авторизации в случае возвращения кода 401
    /// </summary>
    public class UnauthorizedRedirectionMiddleware
    {
        private readonly RequestDelegate _next;

        public UnauthorizedRedirectionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
        {
            await _next(context);

            if (context.Response.StatusCode == 401)
            {
                var redirectUrl = configuration.GetSection("LoginRedirectionPath").Value;

                if (redirectUrl != null)
                {
                    context.Response.Redirect(redirectUrl);
                }
            }
        }
    }
}
