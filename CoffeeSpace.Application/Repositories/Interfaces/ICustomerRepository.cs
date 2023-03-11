using CoffeeSpace.Application.Models.CustomerInfo;

namespace CoffeeSpace.Application.Repositories.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    // Task<IEnumerable<Customer>> GetAllAsync(CancellationToken token = default);
    // IEnumerable<Customer> GetAll();
    // Task<Customer?> GetByIdAsync(Guid id, CancellationToken token = default);
    // Task<bool> CreateAsync(Customer customer, CancellationToken token = default);
    // Task<bool> UpdateAsync(Customer entity, CancellationToken token = default);
    // Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
}