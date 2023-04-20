using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Extensions;
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

    public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var isNotEmpty = await _orderingDbContext.Orders.AnyAsync(cancellationToken);
        if (!isNotEmpty)
        {
            return Enumerable.Empty<Order>();
        }

        return _orderingDbContext.Orders;
    }

    public async Task<IEnumerable<Order>> GetAllByBuyerIdAsync(string buyerId, CancellationToken cancellationToken = default)
    {
        var isNotEmpty = await _orderingDbContext.Orders.AnyAsync(cancellationToken);
        if (!isNotEmpty)
        {
            return Enumerable.Empty<Order>();
        }

        var orders = _orderingDbContext.Orders
            .Where(x => x.BuyerId == buyerId)
            .Include(x => x.Address)
            .Include(x => x.Buyer)
            .Include(x => x.OrderItems);
        
        return orders;
    }

    public async Task<Order?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var order = await _orderingDbContext.Orders.FindAsync(new object?[] { id},  cancellationToken);
        if (order is not null)
        {
            await _orderingDbContext.Orders.LoadDataAsync(order, x => x.OrderItems);
            await _orderingDbContext.Orders.LoadDataAsync(order, x => x.Address);
        }
        
        return order;
    }
    
    public async Task<bool> CreateAsync(Order order, CancellationToken cancellationToken = default)
    {
        await _orderingDbContext.Orders.AddAsync(order, cancellationToken);
        var result = await _orderingDbContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }

    public async Task<Order?> UpdateAsync(Order order, CancellationToken cancellationToken = default)
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

    public async Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var order = await _orderingDbContext.Orders.FindAsync(new object?[] { id }, cancellationToken);
        if (order is null)
        {
            return false;
        }

        _orderingDbContext.Orders.Remove(order);
        var result = await _orderingDbContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }

}