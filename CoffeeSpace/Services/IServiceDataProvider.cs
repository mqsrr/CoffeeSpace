namespace CoffeeSpace.Services;

public interface IServiceDataProvider<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token = default);
    Task<TEntity> GetByIdAsync(Guid id, CancellationToken token = default);
    Task AddAsync(TEntity customer, CancellationToken token = default);
    Task UpdateAsync(TEntity entity, CancellationToken token = default);
    Task Delete(Guid id, CancellationToken token = default);
}