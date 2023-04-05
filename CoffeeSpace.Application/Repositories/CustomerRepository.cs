using CoffeeSpace.Application.Repositories.Abstractions;
using CoffeeSpace.Domain.Models.CustomerInfo;
using CoffeeSpace.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.Application.Repositories;

public sealed class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public CustomerRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var isEmpty = await _applicationDbContext.Customers.AnyAsync(cancellationToken);
        if (!isEmpty)
        {
            return Enumerable.Empty<Customer>();
        }

        return _applicationDbContext.Customers;
    }

    public async Task<Customer?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var customer = await _applicationDbContext.Customers.FindAsync(new object?[] {id}, cancellationToken);

        return customer;
    }

    public async Task<bool> CreateAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        await _applicationDbContext.Customers.AddAsync(customer, cancellationToken);
        var result = await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }

    public async Task<Customer?> UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        var isContains = await _applicationDbContext.Customers.ContainsAsync(customer, cancellationToken);
        if (!isContains)
        {
            return null;
        }

        _applicationDbContext.Customers.Update(customer);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return customer;
    }

    public async Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var customer = await _applicationDbContext.Customers.FindAsync(new object?[] {id}, cancellationToken);
        if (customer is null)
        {
            return false;
        }

        _applicationDbContext.Customers.Remove(customer);
        var result = await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }

    public async Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var customer = await _applicationDbContext.Customers.FirstOrDefaultAsync(c => c.Email == email, cancellationToken);

        return customer;
    }
}