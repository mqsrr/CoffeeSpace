using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using CoffeeSpace.OrderingApi.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.OrderingApi.Application.Repositories;

internal sealed class BuyerRepository : IBuyerRepository
{
    private readonly IOrderingDbContext _orderingDbContext;

    public BuyerRepository(IOrderingDbContext orderingDbContext)
    {
        _orderingDbContext = orderingDbContext;
    }

    public async Task<Buyer?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var buyer = await _orderingDbContext.Buyers
            .AsSplitQuery()
            .Include(buyer => buyer.Orders)!
            .ThenInclude(order => order.OrderItems)
            .Include(buyer => buyer.Orders)!
            .ThenInclude(order => order.Address)
            .FirstOrDefaultAsync(buyer => buyer.Id == id, cancellationToken);

        return buyer;
    }

    public async Task<Buyer?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var buyer = await _orderingDbContext.Buyers
            .AsSplitQuery()
            .Include(buyer => buyer.Orders)!
            .ThenInclude(order => order.OrderItems)
            .Include(buyer => buyer.Orders)!
            .ThenInclude(order => order.Address)
            .FirstOrDefaultAsync(buyer => buyer.Email == email, cancellationToken);

        return buyer;
    }

    public async Task<bool> CreateAsync(Buyer buyer, CancellationToken cancellationToken)
    {
        await _orderingDbContext.Buyers.AddAsync(buyer, cancellationToken);
        int result = await _orderingDbContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }

    public async Task<Buyer?> UpdateAsync(Buyer updatedBuyer, CancellationToken cancellationToken)
    {
        int result = await _orderingDbContext.Buyers
            .Where(buyer => buyer.Id == updatedBuyer.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(buyer => buyer.Email, updatedBuyer.Email)
                .SetProperty(buyer => buyer.Name, updatedBuyer.Name), cancellationToken);

        return result > 0
            ? updatedBuyer
            : null;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        int result = await _orderingDbContext.Buyers
            .Where(buyer => buyer.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return result > 0;
    }
}