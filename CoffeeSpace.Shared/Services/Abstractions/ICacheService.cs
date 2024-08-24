namespace CoffeeSpace.Shared.Services.Abstractions;

public interface ICacheService
{
    public Task<TEntity?> GetAsync<TEntity>(string key, CancellationToken cancellationToken) where TEntity : class;

    Task SetAsync(string key, string entity, CancellationToken cancellationToken);
    
    Task RemoveAsync(string key, CancellationToken cancellationToken);
    
    Task<TEntity?> GetOrCreateAsync<TEntity>(string key, Func<Task<TEntity?>> createEntity, CancellationToken cancellationToken) where TEntity : class;
    
    Task<IEnumerable<TEntity>> GetAllOrCreateAsync<TEntity>(string key, Func<Task<IEnumerable<TEntity>>> createEntity, CancellationToken cancellationToken) where TEntity : class;
}