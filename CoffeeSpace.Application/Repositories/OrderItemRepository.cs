using CoffeeSpace.Application.Repositories.Abstractions;
using CoffeeSpace.Domain.Models.Orders;
using CoffeeSpace.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.Application.Repositories;

public sealed class OrderItemRepository : IOrderItemRepository
{
    private readonly ApplicationDbContext _dbContextContext;

    public OrderItemRepository(ApplicationDbContext dbContextContext)
    {
        _dbContextContext = dbContextContext;
    }

    public async Task<IEnumerable<OrderItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var isEmpty = await _dbContextContext.OrderItems.AnyAsync(cancellationToken);
        if (!isEmpty)
        {
            return Enumerable.Empty<OrderItem>();
        }

        return _dbContextContext.OrderItems;
    }

    public async Task<OrderItem?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var orderItem = await _dbContextContext.OrderItems.FindAsync(new object?[] {id}, cancellationToken);

        return orderItem;
    }

    public Task<IEnumerable<OrderItem>> GetAllByOrderIdAsync(string orderId, CancellationToken cancellationToken = default)
    {
        var orderItems = _dbContextContext.OrderItems.Where(x => x.Id == orderId).AsEnumerable();

        return Task.FromResult(orderItems);
    }

    public async Task<bool> CreateAsync(OrderItem orderItem, CancellationToken cancellationToken = default)
    {
        await _dbContextContext.OrderItems.AddAsync(orderItem, cancellationToken);
        var result = await _dbContextContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }

    public async Task<OrderItem?> UpdateAsync(OrderItem orderItem, CancellationToken cancellationToken = default)
    {
        var isContains = await _dbContextContext.OrderItems.ContainsAsync(orderItem, cancellationToken);
        if (!isContains)
        {
            return null;
        }

        _dbContextContext.OrderItems.Update(orderItem);
        await _dbContextContext.SaveChangesAsync(cancellationToken);

        return orderItem;
    }

    public async Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var orderItem = await _dbContextContext.OrderItems.FindAsync(new object?[] {id}, cancellationToken);
        if (orderItem is null)
        {
            return false;
        }

        _dbContextContext.OrderItems.Remove(orderItem);
        var result = await _dbContextContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }
}