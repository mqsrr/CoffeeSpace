using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllByBuyerIdAsync(Guid buyerId, CancellationToken cancellationToken);
    
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<bool> CreateAsync(Order order, CancellationToken cancellationToken);

    Task<bool> UpdateOrderStatusAsync(Guid id, OrderStatus orderStatus, CancellationToken cancellationToken);
    
    Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken);
}