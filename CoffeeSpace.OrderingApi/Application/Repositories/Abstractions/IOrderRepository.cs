using CoffeeSpace.Domain.Ordering.Orders;

namespace CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;

internal interface IOrderRepository : IRepository<Order>
{
    Task<IEnumerable<Order>> GetAllByBuyerIdAsync(string buyerId, CancellationToken cancellationToken = default);
    
    Task<Order?> UpdateOrderStatusAsync(Order order, OrderStatus orderStatus, CancellationToken cancellationToken = default);
}