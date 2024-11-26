using System.Diagnostics;
using WorkyOne.AppServices.Interfaces.Stores;

namespace WorkyOne.MVC.Middlewares
{
    public class AccessFiltersMiddleware
    {
        private RequestDelegate _next;

        public AccessFiltersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IAccessFiltersStore filtersStore,
            ILogger<AccessFiltersMiddleware> logger
        )
        {
            var id = Guid.NewGuid().ToString();
            logger.LogInformation("{1}: Запрос фильтра доступа", id);

            var timer = new Stopwatch();
            timer.Start();
            await filtersStore.CreateFiltersAsync();
            logger.LogInformation(
                "{1}: Фильтр доступа получен ({2} ms)",
                id,
                timer.ElapsedMilliseconds
            );
            timer.Stop();

            await _next(context);
        }
    }
}
