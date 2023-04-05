using CoffeeSpace.Domain.Models.Orders;

namespace CoffeeSpace.Application.Repositories.Abstractions;

public interface IOrderRepository : IRepository<Order>
{
    Task<IEnumerable<Order>> GetAllByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default);
    
    Task<Order?> GetCustomerByCustomerIdAsync(string customerId, string id, CancellationToken cancellationToken = default);
}