using CoffeeSpace.Data.Models.CustomerInfo;
using CoffeeSpace.Data.Models.Orders;

namespace CoffeeSpace.WebAPI.Services.Repository.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order> CreateAsync(IEnumerable<OrderItem> orderItems, Customer customer, CancellationToken token = default);
    }
}
