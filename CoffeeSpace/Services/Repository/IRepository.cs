namespace CoffeeSpace.Services.Repository;

public interface IRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token = default);
    Task<TEntity> GetByIdAsync(string id, CancellationToken token = default);
    Task AddAsync(TEntity entity, CancellationToken token = default);
    Task DeleteAsync(TEntity entity, CancellationToken token = default);
    Task UpdateAsync(TEntity entity, CancellationToken token = default);
}
