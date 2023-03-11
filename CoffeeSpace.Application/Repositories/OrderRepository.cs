using CoffeeSpace.Application.Context;
using CoffeeSpace.Application.Models.Orders;
using CoffeeSpace.Application.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.Application.Repositories;

public sealed class OrderRepository : IOrderRepository
{
    private readonly ApplicationDb _dbContext;

    public OrderRepository(ApplicationDb applicationDb)
    {
        _dbContext = applicationDb;

        _dbContext.Orders
            .Include(x => x.OrderItems)
            .Include(x => x.Customer)
            .ThenInclude(x => x.Address)
            .Include(x => x.Customer)
            .ThenInclude(x => x.PaymentInfo);
    }

    public async Task<bool> CreateAsync(Order order, CancellationToken token = default)
    {
        OrderItem? result = await _dbContext.OrderItems.SingleOrDefaultAsync(x => x.Title.Equals(order), cancellationToken: token);

        if (await _dbContext.Orders.ContainsAsync(order, token))
            return false;

        await _dbContext.Orders.AddAsync(order, token);
        await _dbContext.SaveChangesAsync(token);

        return true;
    }
    
    public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken token = default) =>
        await _dbContext.Orders.AnyAsync(token) 
            ? _dbContext.Orders 
            : Enumerable.Empty<Order>();

    public IEnumerable<Order> GetAll() => _dbContext.Orders;

    public async Task<Order?> GetByIdAsync(string id, CancellationToken token = default)
    {
        Order? order = await _dbContext.Orders.FindAsync(new object[] { id }, token); 
        
        return order;
    }

    public async Task<bool> UpdateAsync(Order order, CancellationToken token = default)
    {
        if (!await _dbContext.Orders.ContainsAsync(order, token))
            return false;
        
        _dbContext.Update(order);
        await _dbContext.SaveChangesAsync(token);

        return true;
    }

    public async Task<bool> DeleteByIdAsync(string id, CancellationToken token = default)
    {
        Order? order = await _dbContext.Orders.FindAsync(new object?[] { id }, cancellationToken: token);
        
        if (order is null)
            return false;

        _dbContext.Orders.Remove(order);
        
        await _dbContext.SaveChangesAsync(token);

        return true;
    }
}
