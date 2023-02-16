using CoffeeSpace.Data.Context;
using CoffeeSpace.Data.Models.CustomerInfo;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.WebAPI.Services.Repository;

public sealed class CustomerRepository : IRepository<Customer>
{
    private readonly ApplicationDb _dbContext;

    public CustomerRepository(ApplicationDb dbContext)
    {
        _dbContext = dbContext;

        _dbContext.Customers
            .Include(x => x.Address)
            .Include(x => x.PaymentInfo)
            .Include(x => x.Orders)
            .ThenInclude(x => x.OrderItems);
    }
    
    public async Task AddAsync(Customer entity, CancellationToken token = default)
    {
        if (!await _dbContext.Customers.ContainsAsync(entity, token))
            return;

        await _dbContext.AddAsync(entity, token);
        await _dbContext.SaveChangesAsync(token);
    }

    public IEnumerable<Customer> GetAll() => _dbContext.Customers;

    public async Task<Customer> GetByIdAsync(string id, CancellationToken token = default)
    {
        Customer? customer = await _dbContext.Customers.FindAsync(new object[] { id }, cancellationToken: token);

        ArgumentNullException.ThrowIfNull(customer);
        
        return customer;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken token) =>
        await _dbContext.Customers.AnyAsync(token) 
            ? _dbContext.Customers 
            : Enumerable.Empty<Customer>();

    public async Task DeleteAsync(Customer entity, CancellationToken token = default)
    {
        if (!await _dbContext.Customers.ContainsAsync(entity, token)) 
            return;

        _dbContext.Remove(entity);
        await _dbContext.SaveChangesAsync(token);
    }

    public async Task UpdateAsync(Customer entity, CancellationToken token = default)
    {
        if (!await _dbContext.Customers.ContainsAsync(entity, token))
        {
            await _dbContext.Customers.AddAsync(entity, token);
            await _dbContext.SaveChangesAsync(token);

            return;
        }

        _dbContext.Customers.Update(entity);
        await _dbContext.SaveChangesAsync(token);
    }
}