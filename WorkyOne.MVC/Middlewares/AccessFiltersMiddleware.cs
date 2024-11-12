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

        public async Task InvokeAsync(HttpContext context, IAccessFiltersStore filtersStore)
        {
            await filtersStore.CreateFiltersAsync();

            await _next(context);
        }
    }
}
