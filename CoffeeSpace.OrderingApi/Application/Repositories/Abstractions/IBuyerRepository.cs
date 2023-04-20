using CoffeeSpace.Domain.Ordering.CustomerInfo;

namespace CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;

internal interface IBuyerRepository : IRepository<Buyer>
{
    Task<Buyer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}