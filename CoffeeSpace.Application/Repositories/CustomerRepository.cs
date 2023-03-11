using CoffeeSpace.Application.Context;
using CoffeeSpace.Application.Models.CustomerInfo;
using CoffeeSpace.Application.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.Application.Repositories;

public sealed class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDb _dbContext;

    public CustomerRepository(ApplicationDb dbContext)
    {
        _dbContext = dbContext;

        // _dbContext.Customers
        //     .Include(x => x.Address)
        //     .Include(x => x.PaymentInfo)
        //     .Include(x => x.Orders)
        //     .ThenInclude(x => x.OrderItems);
    }

    public async Task<bool> CreateAsync(Customer customer, CancellationToken token = default)
    {
        Customer? result = await _dbContext.Customers.SingleOrDefaultAsync(
            x => x.UserName!.Equals(customer.UserName), cancellationToken: token);

        if (result is not null)
            return false;

        await _dbContext.AddAsync(customer, token);
        await _dbContext.SaveChangesAsync(token);

        return true;
    }

    public IEnumerable<Customer> GetAll() => _dbContext.Customers;

    public async Task<Customer?> GetByIdAsync(string id, CancellationToken token = default)
    {
        Customer? customer = await _dbContext.Customers.FindAsync(new object[] { id }, cancellationToken: token);

        if (customer is null)
            return customer;

        customer.Address =
            (await _dbContext.Addresses.FindAsync(new object?[] { customer.AddressId }, cancellationToken: token))!;
        customer.PaymentInfo =
            (await _dbContext.PaymentInfos.FindAsync(new object?[] { customer.PaymentId }, cancellationToken: token))!;

        return customer;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken token) =>
        await _dbContext.Customers.AnyAsync(token)
            ? _dbContext.Customers
            : Enumerable.Empty<Customer>();

    public async Task<bool> UpdateAsync(Customer customer, CancellationToken token = default)
    {
        if (!await _dbContext.Customers.ContainsAsync(customer, token))
            return false;

        _dbContext.Customers.Update(customer);
        await _dbContext.SaveChangesAsync(token);

        return true;
    }

    public async Task<bool> DeleteByIdAsync(string id, CancellationToken token = default)
    {
        Customer? customer = await _dbContext.Customers.FindAsync(new object?[] { id }, cancellationToken: token);

        if (customer is null)
            return false;

        _dbContext.Remove(customer);
        await _dbContext.SaveChangesAsync(token);

        return true;
    }
}