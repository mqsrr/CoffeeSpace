using CoffeeSpace.Application.Contracts.Requests.Customer;
using CoffeeSpace.Application.Models.CustomerInfo;

namespace CoffeeSpace.Services;

public interface ICustomerService
{
    new Task<Customer> GetByIdAsync(string id, CancellationToken token = default);
    new Task<Customer> GetByEmailAsync(string email, CancellationToken token = default);
    Task<bool> CreateAsync(CreateCustomerRequest request, CancellationToken token = default);
    Task<Customer> UpdateAsync(UpdateCustomerRequest request, CancellationToken token = default);
    Task<bool> DeleteByIdAsync(string id, CancellationToken token = default);
}