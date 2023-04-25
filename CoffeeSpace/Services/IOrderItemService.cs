using CoffeeSpace.Application.Contracts.Requests.OrderItem;
using CoffeeSpace.Application.Models.Orders;

namespace CoffeeSpace.Services;

public interface IOrderItemService
{
    Task<IEnumerable<OrderItem>> GetAllAsync(CancellationToken token = default);
    Task<OrderItem> GetByIdAsync(string id, CancellationToken token = default);
    Task<bool> CreateAsync(CreateOrderItemRequest request, CancellationToken token = default);
    Task<OrderItem> UpdateAsync(UpdateOrderItemRequest request, CancellationToken token = default);
    Task<bool> DeleteByIdAsync(string id, CancellationToken token = default);
}