using CoffeeSpace.Shared.Services.Abstractions;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace CoffeeSpace.Shared.Services;

internal sealed class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    public CacheService(IDistributedCache cache)
    {
        _cache = cache;
    }
    
    public async Task<TEntity?> GetAsync<TEntity>(string key, CancellationToken cancellationToken) where TEntity : class
    {
        string? cachedEntity = await _cache.GetStringAsync(key, token: cancellationToken);
        return cachedEntity is null
            ? null
            : JsonConvert.DeserializeObject<TEntity>(cachedEntity);
    }
    
    public Task SetAsync(string key, string entity, CancellationToken cancellationToken)
    {
        return _cache.SetStringAsync(key, entity, token: cancellationToken);
    }
    
    public Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        return _cache.RemoveAsync(key, cancellationToken);
    }
    
    public async Task<TEntity?> GetOrCreateAsync<TEntity>(
        string key, 
        Func<Task<TEntity?>> createEntity,
        CancellationToken cancellationToken) where TEntity : class
    {
        var cachedEntity = await GetAsync<TEntity>(key, cancellationToken);
        if (cachedEntity is not null)
        {
            return cachedEntity;
        }

        var entity = await createEntity();
        if (entity is null)
        {
            return null;
        }

        string jsonEntity = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });

        await SetAsync(key, jsonEntity, cancellationToken);
        return entity;
    }

    public async Task<IEnumerable<TEntity>> GetAllOrCreateAsync<TEntity>(
        string key,
        Func<Task<IEnumerable<TEntity>>> createEntity,
        CancellationToken cancellationToken) where TEntity : class
    {
        var cachedEntity = await GetAsync<IEnumerable<TEntity>>(key, cancellationToken);
        if (cachedEntity is not null && cachedEntity.TryGetNonEnumeratedCount(out int count) && count > 0)
        {
            return cachedEntity;
        }

        var entity = await createEntity();
        if (entity.TryGetNonEnumeratedCount(out count) && count == 0)
        {
            return Enumerable.Empty<TEntity>();
        }

        string jsonEntity = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });

        await SetAsync(key, jsonEntity, cancellationToken);
        return entity;

    }
}