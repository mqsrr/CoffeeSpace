using CoffeeSpace.Application.Models.Orders;

namespace CoffeeSpace.Application.Repositories.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    // Task<IEnumerable<Order>> GetAllAsync(CancellationToken token = default);
    // IEnumerable<Order> GetAll();
    // Task<OrderItem?> GetByIdAsync(Guid id, CancellationToken token = default);
    // Task<bool> CreateAsync(OrderItem customer, CancellationToken token = default);
    // Task<bool> UpdateAsync(OrderItem entity, CancellationToken token = default);
    // Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
}