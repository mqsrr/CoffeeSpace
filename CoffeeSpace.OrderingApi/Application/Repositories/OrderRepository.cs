using CoffeeSpace.Core.Extensions;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using CoffeeSpace.OrderingApi.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.OrderingApi.Application.Repositories;

internal sealed class OrderRepository : IOrderRepository
{
    private readonly IOrderingDbContext _orderingDbContext;

    public OrderRepository(IOrderingDbContext orderingDbContext)
    {
        _orderingDbContext = orderingDbContext;
    }

    public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken)
    {
        var isNotEmpty = await _orderingDbContext.Orders.AnyAsync(cancellationToken);
        if (!isNotEmpty)
        {
            return Enumerable.Empty<Order>();
        }

        return _orderingDbContext.Orders;
    }

    public async Task<IEnumerable<Order>> GetAllByBuyerIdAsync(string buyerId, CancellationToken cancellationToken)
    {
        var isNotEmpty = await _orderingDbContext.Orders.AnyAsync(cancellationToken);
        if (!isNotEmpty)
        {
            return Enumerable.Empty<Order>();
        }

        var orders = _orderingDbContext.Orders
            .Where(x => x.BuyerId == buyerId)
            .Include(x => x.Address)
            .Include(x => x.OrderItems);
        
        return orders;
    }
    
    public async Task<Order?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var order = await _orderingDbContext.Orders.FindAsync(new object?[] {id},  cancellationToken);
        if (order is not null)
        {
            await _orderingDbContext.Orders.LoadDataAsync(order, x => x.OrderItems);
            await _orderingDbContext.Orders.LoadDataAsync(order, x => x.Address);
        }
        
        return order;
    }
    
    public async Task<bool> CreateAsync(Order order, CancellationToken cancellationToken)
    {
        var buyer = await _orderingDbContext.Buyers.FindAsync(new object?[] {order.BuyerId}, cancellationToken);
        if (buyer is null)
        {
            return false;
        }
        
        await _orderingDbContext.Orders.AddAsync(order, cancellationToken);
        var result = await _orderingDbContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }

    public async Task<Order?> UpdateAsync(Order order, CancellationToken cancellationToken)
    {
        var isContains = await _orderingDbContext.Orders.ContainsAsync(order, cancellationToken);
        if (!isContains)
        {
            return null;
        }

        _orderingDbContext.Orders.Update(order);
        await _orderingDbContext.SaveChangesAsync(cancellationToken);
        
        return order;
    }

    public async Task<bool> UpdateOrderStatusAsync(string id, OrderStatus orderStatus, CancellationToken cancellationToken)
    {
        var result = await _orderingDbContext.Orders
            .Where(order => order.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(order => order.Status, orderStatus), cancellationToken);
        
        return result > 0;
    }

    public async Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken)
    {
        var result = await _orderingDbContext.Orders
            .Where(order => order.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return result > 0;
    }

}