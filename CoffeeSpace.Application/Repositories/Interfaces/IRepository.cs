namespace CoffeeSpace.Application.Repositories.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token = default);
    IEnumerable<TEntity> GetAll();
    Task<TEntity?> GetByIdAsync(string id, CancellationToken token = default);
    Task<bool> CreateAsync(TEntity order, CancellationToken token = default);
    Task<bool> UpdateAsync(TEntity customer, CancellationToken token = default);
    Task<bool> DeleteByIdAsync(string id, CancellationToken token = default);
}
