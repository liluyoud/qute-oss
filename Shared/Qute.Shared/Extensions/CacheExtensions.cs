using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Qute.Shared.Extensions;

public static class CacheExtensions
{
    public static async Task<T?> GetAsync<T>(this IDistributedCache cache, string key)
    {
        var data = await cache.GetStringAsync(key);
        if (data == null)
        {
            return default;
        }
        return JsonSerializer.Deserialize<T>(data);
    }

    public static async Task SetAsync<T>(this IDistributedCache cache, string key, T value, TimeSpan? expiration = null)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(1)
        };
        var data = JsonSerializer.Serialize(value);
        await cache.SetStringAsync(key, data, options);
    }

    /// <summary>
    /// Retrieves a value from the cache or creates it using the provided function.
    /// </summary>
    /// <typeparam name="T">The type of the value to be stored.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <param name="getMethod">The function to create the data if it does not exist in the cache.</param>
    /// <param name="expiration">The cache expiration time.</param>
    /// <returns>The stored or newly created value.</returns>
    public static async Task<T?> GetOrCreateAsync<T>(
        this IDistributedCache cache,
        string key,
        Func<Task<T>> getMethod,
        TimeSpan? expiration,
        CancellationToken cancellation = default)
    {
        // Attempt to retrieve the value from the cache
        var cachedData = await cache.GetStringAsync(key);
        if (string.IsNullOrEmpty(cachedData))
        {
            var data = await getMethod();
            if (data is not null)
            {
                string objectJson = JsonSerializer.Serialize(data);
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(1)
                };
                await cache.SetStringAsync(key, objectJson, options, cancellation);
                return data;
            }
            return default!;
        }
        // If the data exists in the cache, deserialize and return it
        return JsonSerializer.Deserialize<T>(cachedData);
    }
}
