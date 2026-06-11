using Microsoft.Extensions.Caching.Distributed;

namespace SaleApi.Extensions;

public static class DistributedCacheExtensions
{
    public static Task RemoveAsync(this IDistributedCache cache, string key, CancellationToken cancellationToken = default)
    {
        cache.Remove(key);
        return Task.CompletedTask;
    }
}
