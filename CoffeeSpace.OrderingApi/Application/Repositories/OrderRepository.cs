using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using CoffeeSpace.OrderingApi.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.OrderingApi.Application.Repositories;

internal sealed class OrderRepository : IOrderRepository
{
    private readonly OrderingDbContext _orderingDbContext;

    public OrderRepository(OrderingDbContext orderingDbContext)
    {
        _orderingDbContext = orderingDbContext;
    }
    
    public async Task<IEnumerable<Order>> GetAllByBuyerIdAsync(string buyerId, CancellationToken cancellationToken)
    {
        bool isNotEmpty = await _orderingDbContext.Orders.AnyAsync(cancellationToken);
        if (!isNotEmpty)
        {
            return Enumerable.Empty<Order>();
        }

        var orders = await _orderingDbContext.Orders
            .Where(order => order.BuyerId == buyerId)
            .Include(order => order.OrderItems)
            .Include(order => order.Address)
            .ToListAsync(cancellationToken);
        
        return orders;
    }
    
    public Task<Order?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var order = _orderingDbContext.Orders
            .Include(order => order.OrderItems)
            .Include(order => order.Address)
            .FirstOrDefaultAsync(order => order.Id == id, cancellationToken);
        
        return order;
    }
    
    public async Task<bool> CreateAsync(Order order, CancellationToken cancellationToken)
    {
        await _orderingDbContext.Orders.AddAsync(order, cancellationToken);
        int result = await _orderingDbContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }
    
    public async Task<bool> UpdateOrderStatusAsync(string id, OrderStatus orderStatus, CancellationToken cancellationToken)
    {
        int result = await _orderingDbContext.Orders
            .Where(order => order.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(order => order.Status, orderStatus), cancellationToken);
        
        return result > 0;
    }

    public async Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken)
    {
        int result = await _orderingDbContext.Orders
            .Where(order => order.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return result > 0;
    }

}