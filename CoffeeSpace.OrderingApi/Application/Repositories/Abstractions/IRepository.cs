namespace CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(string id, CancellationToken cancellationToken);
    
    Task<bool> CreateAsync(TEntity entity, CancellationToken cancellationToken);
    
    Task<TEntity?> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    
    Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken);
}