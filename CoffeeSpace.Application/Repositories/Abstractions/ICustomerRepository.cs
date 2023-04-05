using CoffeeSpace.Domain.Models.CustomerInfo;

namespace CoffeeSpace.Application.Repositories.Abstractions;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}