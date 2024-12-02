﻿using System.Diagnostics;
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
            var timer = new Stopwatch();
            timer.Start();
            await filtersStore.CreateFiltersAsync();
            timer.Stop();
            if (timer.ElapsedMilliseconds > 500)
            {
                logger.LogDebug("Фильтр доступа получен ({1} ms)", timer.ElapsedMilliseconds);
            }

            await _next(context);
        }
    }
}
