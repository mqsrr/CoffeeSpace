using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllByBuyerIdAsync(string buyerId, CancellationToken cancellationToken);
    
    Task<Order?> GetByIdAsync(string id, CancellationToken cancellationToken);
    
    Task<bool> CreateAsync(Order order, CancellationToken cancellationToken);

    Task<bool> UpdateOrderStatusAsync(string id, OrderStatus orderStatus, CancellationToken cancellationToken);
    
    Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken);
}