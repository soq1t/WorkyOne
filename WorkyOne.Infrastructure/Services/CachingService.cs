using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using WorkyOne.AppServices.Interfaces.Services.Common;

namespace WorkyOne.Infrastructure.Services
{
    /// <summary>
    /// Сервис, работающий с кешем
    /// </summary>
    public class CachingService : ICachingService
    {
        private readonly IDistributedCache _cache;

        public CachingService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetAsync<T>(
            string key,
            Func<Task<T?>> function,
            TimeSpan duration,
            CancellationToken cancellation = default
        )
            where T : class
        {
            var prefix = typeof(T).Name;

            string prefixedKey = prefix + "_" + key;

            string? json;
            try
            {
                json = await _cache.GetStringAsync(prefixedKey, cancellation);
            }
            catch (Exception)
            {
                json = null;
            }

            if (json == null)
            {
                var result = await function.Invoke();
                await SaveAsync(key, result, duration, cancellation);

                return result;
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        public async Task SaveAsync<T>(
            string key,
            T value,
            TimeSpan duration,
            CancellationToken cancellation = default
        )
        {
            var prefix = typeof(T).Name;
            var prefixedKey = prefix + "_" + key;
            var json = JsonConvert.SerializeObject(value);

            try
            {
                await _cache.SetStringAsync(
                    prefixedKey,
                    json,
                    new DistributedCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = duration
                    },
                    cancellation
                );
            }
            catch (Exception) { }
        }
    }
}
