namespace CoffeeSpace.Shared.Services.Abstractions;

public interface ICacheService
{
    TEntity? HashScan<TEntity>(string hashKey, string pattern) where TEntity : class;
    
    Task<dynamic?> HashGetAsync(string hashKey, string key, Type type);
    
    Task<TEntity?> HashGetAsync<TEntity>(string hashKey, string key) where TEntity : class;
    
    Task<TEntity?> GetAsync<TEntity>(string key) where TEntity : class;
    
    Task<bool> SetAsync(string key, string entity);
    
    Task<bool> HashSetAsync(string hashKey, string key, string entity);
    
    Task<string?> RemoveAsync(string key);
    
    Task<bool> HashRemoveAsync(string hashKey, string? key);

    Task HashRemoveAllAsync(string hashKey, string? pattern = null);

    Task<TEntity?> GetOrCreateAsync<TEntity>(string key, Func<Task<TEntity?>> createEntity) where TEntity : class;
    
    Task<IEnumerable<TEntity>> GetAllOrCreateAsync<TEntity>(string key, Func<Task<IEnumerable<TEntity>>> createEntity) where TEntity : class;
}