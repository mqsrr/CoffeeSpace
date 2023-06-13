using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.OrderingApi.Application.Services.Abstractions;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAllByBuyerIdAsync(string buyerId, CancellationToken cancellationToken);
    
    Task<Order?> GetByIdAsync(string id, string buyerId, CancellationToken cancellationToken);
    
    Task<bool> CreateAsync(Order order, CancellationToken cancellationToken);
    
    Task<Order?> UpdateAsync(Order order, CancellationToken cancellationToken);
    
    Task<bool> DeleteByIdAsync(string id, string buyerId, CancellationToken cancellationToken);

}