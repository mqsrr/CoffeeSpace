using CoffeeSpace.Domain.Ordering.BuyerInfo;

namespace CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;

public interface IBuyerRepository : IRepository<Buyer>
{
    Task<Buyer?> GetByEmailAsync(string email, CancellationToken cancellationToken);
}