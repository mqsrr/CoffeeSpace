using CoffeeSpace.Application.Context;
using CoffeeSpace.Application.Models.Orders;
using CoffeeSpace.Application.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CoffeeSpace.Application.Repositories;

public sealed class OrderItemRepository : IOrderItemRepository
{
    private readonly ApplicationDb _dbContext;

    public OrderItemRepository(ApplicationDb applicationDb) => _dbContext = applicationDb;

    public async Task<bool> CreateAsync(OrderItem orderItem, CancellationToken token = default)
    {
        OrderItem? result = await _dbContext.OrderItems.SingleOrDefaultAsync(x => x.Title.Equals(orderItem), cancellationToken: token);
        
        if (result is not null)
            return false;

        await _dbContext.OrderItems.AddAsync(orderItem, token);
        await _dbContext.SaveChangesAsync(token);

        return true;
    }

    public async Task<IEnumerable<OrderItem>> GetAllAsync(CancellationToken token = default)
        => await _dbContext.OrderItems.AnyAsync(token)
                ? _dbContext.OrderItems
                : Enumerable.Empty<OrderItem>();

    public IEnumerable<OrderItem> GetAll()
        => _dbContext.OrderItems.Any()
            ? _dbContext.OrderItems
            : Enumerable.Empty<OrderItem>();

    public async Task<OrderItem?> GetByIdAsync(string id, CancellationToken token = default)
    {
        OrderItem? orderItem = await _dbContext.OrderItems.FindAsync(new object[] { id }, cancellationToken: token);
        
        return orderItem;
    }

    public async Task<bool> UpdateAsync(OrderItem orderItem, CancellationToken token = default)
    {
        OrderItem? searchable =
            await _dbContext.OrderItems.FindAsync(new object?[] { orderItem.Id }, cancellationToken: token); 
        
        if (searchable is null)
            return false;
        
        _dbContext.OrderItems.Update(orderItem);

        await _dbContext.SaveChangesAsync(token);

        return true;
    }

    public async Task<bool> DeleteByIdAsync(string id, CancellationToken token = default)
    {
        OrderItem? orderItem = await _dbContext.OrderItems.FindAsync(new object?[] { id }, cancellationToken: token);
        
        if (orderItem is null)
            return false;

        _dbContext.OrderItems.Remove(orderItem);

        await _dbContext.SaveChangesAsync(token);

        return true;
    }
}