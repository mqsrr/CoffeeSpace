using CoffeeSpace.Domain.Ordering.CustomerInfo;

namespace CoffeeSpace.OrderingApi.Application.Services.Abstractions;

public interface IBuyerService
{
    Task<Buyer?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    
    Task<Buyer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    
    Task<bool> CreateAsync(Buyer buyer, CancellationToken cancellationToken = default);
    
    Task<Buyer?> UpdateAsync(Buyer buyer, CancellationToken cancellationToken = default);
    
    Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken = default);
}