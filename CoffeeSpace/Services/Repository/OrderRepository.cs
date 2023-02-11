using CoffeeSpace.Data.Context;
using CoffeeSpace.Data.Models.CustomerInfo;
using CoffeeSpace.Data.Models.Orders;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.Services.Repository;

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

    public async Task AddAsync(Order entity, CancellationToken token = default)
    {
        if (await _dbContext.Orders.ContainsAsync(entity, token))
            return;

        await _dbContext.Orders.AddAsync(entity, token);
        await _dbContext.SaveChangesAsync(token);
    }

    public Task<Order> CreateAsync(IEnumerable<OrderItem> orderItems, Customer customer,
        CancellationToken token = default)
    {
        Order order = new Order
        {
            Customer = customer,
            CustomerId = customer.Id,
            OrderItems = orderItems,
            Status = OrderStatus.Submitted
        };
        
        return Task.FromResult(order);
    }

    public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken token = default) =>
        await _dbContext.Orders.AnyAsync(token) 
            ? _dbContext.Orders 
            : Enumerable.Empty<Order>();

    public async Task<Order> GetByIdAsync(string id, CancellationToken token = default)
    {
        Order order = await _dbContext.Orders.FindAsync(new object[] { id }, token); 
        
        ArgumentNullException.ThrowIfNull(order);
        
        return order;
    }

    public async Task DeleteAsync(Order entity, CancellationToken token = default)
    {
        if (!await _dbContext.Orders.ContainsAsync(entity, token))
            return;

        _dbContext.Orders.Remove(entity);
        
        await _dbContext.SaveChangesAsync(token);
    }

    public async Task UpdateAsync(Order entity, CancellationToken token = default)
    {
        if (!await _dbContext.Orders.ContainsAsync(entity, token))
        {
            await _dbContext.Orders.AddAsync(entity, token);
            await _dbContext.SaveChangesAsync(token);
            
            return;
        }
        
        _dbContext.Update(entity);
        
        await _dbContext.SaveChangesAsync(token);
    }
}
