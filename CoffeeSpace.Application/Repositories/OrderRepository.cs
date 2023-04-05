using CoffeeSpace.Application.Repositories.Abstractions;
using CoffeeSpace.Domain.Models.Orders;
using CoffeeSpace.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.Application.Repositories;

public sealed class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public OrderRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var isEmpty = await _applicationDbContext.Orders.AnyAsync(cancellationToken);
        if (isEmpty)
        {
            return Enumerable.Empty<Order>();
        }

        return _applicationDbContext.Orders;
    }

    public async Task<IEnumerable<Order>> GetAllByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default)
    {
        var isEmpty = await _applicationDbContext.Orders.AnyAsync(cancellationToken);
        if (isEmpty)
        {
            return Enumerable.Empty<Order>();
        }

        var orders = _applicationDbContext.Orders
            .Include(x => x.Customer)
            .Where(order => order.CustomerId == customerId);

        return orders;
    }

    public async Task<Order?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var order = await _applicationDbContext.Orders.FindAsync(new object?[] {id}, cancellationToken: cancellationToken);

        return order;
    }

    public async Task<Order?> GetCustomerByCustomerIdAsync(string customerId, string id, CancellationToken cancellationToken = default)
    {
        var order = await _applicationDbContext.Orders.SingleOrDefaultAsync(x => x.CustomerId == customerId, cancellationToken);

        return order;
    }

    public async Task<bool> CreateAsync(Order order, CancellationToken cancellationToken = default)
    {
        await _applicationDbContext.Orders.AddAsync(order, cancellationToken);
        var result = await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }

    public async Task<Order?> UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        var isContains = await _applicationDbContext.Orders.ContainsAsync(order, cancellationToken);
        if (isContains)
        {
            return null;
        }

        _applicationDbContext.Update(order);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return order;
    }

    public async Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var order = await _applicationDbContext.Orders.FindAsync(new object?[] {id}, cancellationToken);
        if (order is null)
        {
            return false;
        }

        _applicationDbContext.Orders.Remove(order);
        var result = await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }

}