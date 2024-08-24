using CoffeeSpace.Domain.Ordering.BuyerInfo;

namespace CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;

public interface IBuyerRepository
{
    Task<Buyer?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Buyer?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    
    Task<bool> CreateAsync(Buyer entity, CancellationToken cancellationToken);

    Task<Buyer?> UpdateAsync(Buyer updatedBuyer, CancellationToken cancellationToken);

    Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken);

}