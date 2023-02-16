using CoffeeSpace.Data.Models.CustomerInfo;
using CoffeeSpace.Data.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoffeeSpace.WebAPI.Services.Repository
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order> CreateAsync(IEnumerable<OrderItem> orderItems, Customer customer, CancellationToken token = default);
    }
}
