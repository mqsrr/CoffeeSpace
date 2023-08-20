using CoffeeSpace.Core.Extensions;
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

    public async Task<Buyer?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var buyer = await _orderingDbContext.Buyers.FindAsync(new object?[] {id}, cancellationToken);
        if (buyer is null)
        {
            return buyer;
        }

        await _orderingDbContext.Buyers.LoadDataAsync(buyer, b => b.Orders!);
        await _orderingDbContext.Orders.LoadDataAsync(buyer.Orders!, o => o.OrderItems);
        await _orderingDbContext.Orders.LoadDataAsync(buyer.Orders!, o => o.Address);

        return buyer;
    }

    public async Task<Buyer?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var buyer = await _orderingDbContext.Buyers.FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
        if (buyer is null)
        {
            return buyer;
        }

        await _orderingDbContext.Buyers.LoadDataAsync(buyer, b => b.Orders!);
        await _orderingDbContext.Orders.LoadDataAsync(buyer.Orders!, o => o.OrderItems);
        await _orderingDbContext.Orders.LoadDataAsync(buyer.Orders!, o => o.Address);

        return buyer;
    }

    public async Task<bool> CreateAsync(Buyer buyer, CancellationToken cancellationToken)
    {
        await _orderingDbContext.Buyers.AddAsync(buyer, cancellationToken);
        int result = await _orderingDbContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }

    public async Task<Buyer?> UpdateAsync(Buyer buyer, CancellationToken cancellationToken)
    {
        var buyerToUpdate = await _orderingDbContext.Buyers.FindAsync(new object[]{buyer.Id}, cancellationToken);
        if (buyerToUpdate is null)
        {
            return null;
        }

        _orderingDbContext.Buyers.Update(buyer);
        await _orderingDbContext.SaveChangesAsync(cancellationToken);

        return buyer;
    }

    public async Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken)
    {
        
        int result = await _orderingDbContext.Buyers
            .Where(buyer => buyer.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        return result > 0;
    }
}