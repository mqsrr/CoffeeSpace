namespace CoffeeSpace.Core.Services.Abstractions;

public interface ICacheService<TEntity>
    where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync(string key, CancellationToken cancellationToken);

    Task<TEntity?> GetAsync(string key, CancellationToken cancellationToken);

    Task SetAsync(string key, string jsonEntity, CancellationToken cancellationToken);
    
    Task RemoveAsync(string key, CancellationToken cancellationToken);

    Task<TEntity?> GetOrCreateAsync(string key, Func<Task<TEntity?>> createEntity, CancellationToken cancellationToken);

    Task<IEnumerable<TEntity>> GetAllOrCreateAsync(string key, Func<Task<IEnumerable<TEntity>>> createEntity, CancellationToken cancellationToken);
}