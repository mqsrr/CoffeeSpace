namespace CoffeeSpace.Application.Repositories.Abstractions;

public interface IRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    
    Task<TEntity?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    
    Task<bool> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    Task<TEntity?> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken = default);
}