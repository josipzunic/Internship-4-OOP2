using Domain.Persistence.Companies;
using Infrastructure.Database.Configurations;
using Infrastructure.Persistence;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Common;

public class CacheService : ICacheServices
{
    private readonly IMemoryCache _cache;
    public CacheService(IMemoryCache cache)
    {
        _cache = cache;
    }
    
    public Task<TValue> Get<TValue>(string key)
    {
        _cache.TryGetValue(key, out TValue? value);
        return Task.FromResult(value);
    }

    public Task Set<TValue>(string key, TValue value, TimeSpan expiration)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };
        
        _cache.Set(key, value, options);
        return Task.CompletedTask;
    }
}