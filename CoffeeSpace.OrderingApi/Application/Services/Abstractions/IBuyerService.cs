using CoffeeSpace.Domain.Ordering.BuyerInfo;

namespace CoffeeSpace.OrderingApi.Application.Services.Abstractions;

public interface IBuyerService
{
    Task<Buyer?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<Buyer?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    
    Task<bool> CreateAsync(Buyer buyer, CancellationToken cancellationToken);
    
    Task<Buyer?> UpdateAsync(Buyer buyer, CancellationToken cancellationToken);
    
    Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken);
}