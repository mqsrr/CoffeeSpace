using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.OrderingApi.Application.Services.Abstractions;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAllByBuyerIdAsync(Guid buyerId, CancellationToken cancellationToken);
    
    Task<Order?> GetByIdAsync(Guid id, Guid buyerId, CancellationToken cancellationToken);
    
    Task<bool> CreateAsync(Order order, CancellationToken cancellationToken);
    
    Task<bool> DeleteByIdAsync(Guid id, Guid buyerId, CancellationToken cancellationToken);

}