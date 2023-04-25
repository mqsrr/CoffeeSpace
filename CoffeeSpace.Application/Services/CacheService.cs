using CoffeeSpace.Application.Services.Abstractions;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace CoffeeSpace.Application.Services;

public sealed class CacheService<TEntity> : ICacheService<TEntity>
    where TEntity : class, new()
{
    private readonly IDistributedCache _distributedCache;
    private readonly DistributedCacheEntryOptions _options;

    public CacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
        _options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
            SlidingExpiration = TimeSpan.FromMinutes(3)
        };
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(string key, CancellationToken cancellationToken)
    {
        string? cachedOrderItem = await _distributedCache.GetStringAsync(key, cancellationToken);
        if (cachedOrderItem is null)
        {
            return Enumerable.Empty<TEntity>();
        }

        return JsonConvert.DeserializeObject<IEnumerable<TEntity>>(cachedOrderItem)!;
    }

    public async Task<TEntity?> GetAsync(string key, CancellationToken cancellationToken)
    {
        string? cachedOrderItem = await _distributedCache.GetStringAsync(key, cancellationToken);
        if (cachedOrderItem is null)
        {
            return null;
        }

        return JsonConvert.DeserializeObject<TEntity>(cachedOrderItem);
    }

    public Task SetAsync(string key, string jsonEntity, CancellationToken cancellationToken)
    {
        return _distributedCache.SetStringAsync(key, jsonEntity, _options, cancellationToken);
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        return _distributedCache.RemoveAsync(key, cancellationToken);
    }

    public async Task<TEntity?> GetOrCreateAsync(string key, Func<Task<TEntity?>> createEntity,
        CancellationToken cancellationToken)
    {
        var cachedEntity = await GetAsync(key, cancellationToken);
        if (cachedEntity is not null)
        {
            return cachedEntity;
        }

        var entity = await createEntity();
        if (entity is null)
        {
            return null;
        }

        var jsonEntity = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });

        await SetAsync(key, jsonEntity, cancellationToken);
        return entity;
    }

    public async Task<IEnumerable<TEntity>> GetAllOrCreateAsync(string key, Func<Task<IEnumerable<TEntity>>> createEntity,
        CancellationToken cancellationToken)
    {
        var cachedEntity = await GetAllAsync(key, cancellationToken);
        if (cachedEntity.TryGetNonEnumeratedCount(out int count) && count > 0)
        {
            return cachedEntity;
        }

        var entity = await createEntity();
        if (entity.TryGetNonEnumeratedCount(out count) && count == 0)
        {
            return Enumerable.Empty<TEntity>();
        }

        var jsonEntity = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });

        await SetAsync(key, jsonEntity, cancellationToken);
        return entity;
    }
}