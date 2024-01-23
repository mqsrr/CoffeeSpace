using CoffeeSpace.Shared.Services.Abstractions;
using CoffeeSpace.Shared.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CoffeeSpace.Shared.Services;

internal sealed class CacheService : ICacheService
{
    private readonly IDatabaseAsync _redisDatabase;
    private readonly CacheSettings _cacheSettings;

    public CacheService(IConnectionMultiplexer connectionMultiplexer, IOptions<CacheSettings> cacheSettings)
    {
        _cacheSettings = cacheSettings.Value;
        _redisDatabase = connectionMultiplexer.GetDatabase();
    }

    public TEntity? HashScan<TEntity>(string hashKey, string pattern) where TEntity : class
    {
        var entry = _redisDatabase.HashScanAsync(hashKey, pattern).ToBlockingEnumerable().FirstOrDefault();
        if (entry.Value.IsNullOrEmpty)
        {
            return null;
        }

        return entry.Value.HasValue
            ? JsonConvert.DeserializeObject<TEntity>(entry.Value!)
            : null;
    }
    
    public async Task<dynamic?> HashGetAsync(string hashKey, string key, Type type)
    {
        string? cachedEntity = await _redisDatabase.HashGetAsync(hashKey, key);
        return cachedEntity is null
            ? null
            : JsonConvert.DeserializeObject(cachedEntity, type);
    }
    
    public async Task<TEntity?> HashGetAsync<TEntity>(string hashKey, string key) where TEntity : class
    {
        return await HashGetAsync(hashKey, key, typeof(TEntity)) as TEntity;
    }
    
    public async Task<TEntity?> GetAsync<TEntity>(string key) where TEntity : class
    {
        string? cachedEntity = await _redisDatabase.StringGetAsync(key);
        return cachedEntity is null
            ? null
            : JsonConvert.DeserializeObject<TEntity>(cachedEntity);
    }
    
    public async Task<bool> SetAsync(string key, string entity)
    {
        bool isSuccess = await _redisDatabase.StringSetAsync(key, entity);
        await _redisDatabase.KeyExpireAsync(key, _cacheSettings.ExpireTime, ExpireWhen.HasNoExpiry);

        return isSuccess;
    }
    
    public async Task<bool> HashSetAsync(string hashKey, string key, string entity)
    {
        bool isSuccess = await _redisDatabase.HashSetAsync(hashKey, key, entity);
        await _redisDatabase.KeyExpireAsync(hashKey, _cacheSettings.ExpireTime, ExpireWhen.HasNoExpiry);

        return isSuccess;
    }
    
    public async Task<string?> RemoveAsync(string key)
    {
        var redisValue = await _redisDatabase.StringGetDeleteAsync(key);
        return redisValue;
    }
    
    public async Task<bool> HashRemoveAsync(string hashKey, string? key)
    {
        bool isDeleted = await _redisDatabase.HashDeleteAsync(hashKey, key);
        return isDeleted;
    }
    
    public async Task HashRemoveAllAsync(string hashKey, string? pattern = "*")
    {
        var keys = _redisDatabase.HashScanAsync(hashKey, pattern);
        await foreach (var cacheKey in keys)
        {
            await _redisDatabase.HashDeleteAsync(hashKey, cacheKey.Value);
        }
    }
    
    public async Task<TEntity?> GetOrCreateAsync<TEntity>(
        string key, 
        Func<Task<TEntity?>> createEntity) where TEntity : class
    {
        var cachedEntity = await GetAsync<TEntity>(key);
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

        await SetAsync(key, jsonEntity);
        return entity;
    }

    public async Task<IEnumerable<TEntity>> GetAllOrCreateAsync<TEntity>(string key, Func<Task<IEnumerable<TEntity>>> createEntity) where TEntity : class
    {
        var cachedEntity = await GetAsync<IEnumerable<TEntity>>(key);
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

        await SetAsync(key, jsonEntity);
        return entity;

    }
}