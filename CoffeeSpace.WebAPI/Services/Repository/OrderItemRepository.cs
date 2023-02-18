using CoffeeSpace.Data.Context;
using CoffeeSpace.Data.Models.Orders;
using CoffeeSpace.WebAPI.Services.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.WebAPI.Services.Repository;

public sealed class OrderItemRepository : IRepository<OrderItem>
{
    private readonly ApplicationDb _dbContext;

    public OrderItemRepository(ApplicationDb applicationDb) => _dbContext = applicationDb;

    public async Task AddAsync(OrderItem entity, CancellationToken token = default)
    {
        if (await _dbContext.OrderItems.ContainsAsync(entity, token))
            return;
        
        await _dbContext.OrderItems.AddAsync(entity, token);
        await _dbContext.SaveChangesAsync(token);
    }

    public async Task<IEnumerable<OrderItem>> GetAllAsync(CancellationToken token = default) =>
        await _dbContext.OrderItems.AnyAsync(token)
            ? _dbContext.OrderItems
            : Enumerable.Empty<OrderItem>();

    public IEnumerable<OrderItem> GetAll() => _dbContext.OrderItems;

    public async Task<OrderItem> GetByIdAsync(string id, CancellationToken token = default)
    {
        OrderItem? orderItem = await _dbContext.OrderItems.FindAsync(new object[] { id }, cancellationToken: token);

        ArgumentNullException.ThrowIfNull(orderItem);

        return orderItem;
    }

    public async Task DeleteAsync(OrderItem entity, CancellationToken token = default)
    {
        if (!await _dbContext.OrderItems.ContainsAsync(entity, token))
            return;

        _dbContext.OrderItems.Remove(entity);

        await _dbContext.SaveChangesAsync(token);
    }
    
    public async Task UpdateAsync(OrderItem entity, CancellationToken token = default)
    {
        if (!await _dbContext.OrderItems.ContainsAsync(entity, token))
        {
            await _dbContext.OrderItems.AddAsync(entity, token);
            await _dbContext.SaveChangesAsync(token);
            
            return;
        }
        
        _dbContext.OrderItems.Update(entity);

        await _dbContext.SaveChangesAsync(token);
    }
}