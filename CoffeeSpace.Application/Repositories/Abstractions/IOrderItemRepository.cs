using CoffeeSpace.Domain.Models.Orders;

namespace CoffeeSpace.Application.Repositories.Abstractions;

public interface IOrderItemRepository : IRepository<OrderItem>
{
    Task<IEnumerable<OrderItem>> GetAllByOrderIdAsync(string orderId, CancellationToken cancellationToken = default);
}