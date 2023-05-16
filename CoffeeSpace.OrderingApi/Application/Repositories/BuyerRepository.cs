using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.OrderingApi.Application.Extensions;
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

    public async Task<IEnumerable<Buyer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var isNotEmpty = await _orderingDbContext.Buyers.AnyAsync(cancellationToken);
        if (!isNotEmpty)
        {
            return Enumerable.Empty<Buyer>();
        }

        return _orderingDbContext.Buyers;
    }

    public async Task<Buyer?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var buyer = await _orderingDbContext.Buyers.FindAsync(new object?[] {id}, cancellationToken);
        if (buyer is not null)
        {
            await _orderingDbContext.Buyers
                .LoadDataAsync(buyer, x => x.Orders!);

            await _orderingDbContext.Orders
                .LoadDataAsync(buyer.Orders!, x => x.OrderItems);
        
            await _orderingDbContext.Orders
                .LoadDataAsync(buyer.Orders!, x => x.Address!);
        }
        
        return buyer;
    }
    
    public async Task<Buyer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var buyer = await _orderingDbContext.Buyers.FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
        if (buyer is not null)
        {
            await _orderingDbContext.Buyers.LoadDataAsync(buyer, x => x.Orders!);
            await _orderingDbContext.Orders.LoadDataAsync(buyer.Orders!, x => x.OrderItems);
            await _orderingDbContext.Orders.LoadDataAsync(buyer.Orders!, x => x.Address!);
        }

        return buyer;
    }

    public async Task<bool> CreateAsync(Buyer buyer, CancellationToken cancellationToken = default)
    {
        await _orderingDbContext.Buyers.AddAsync(buyer, cancellationToken);
        var result = await _orderingDbContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }

    public async Task<Buyer?> UpdateAsync(Buyer buyer, CancellationToken cancellationToken = default)
    {
        var isContains = await _orderingDbContext.Buyers.ContainsAsync(buyer, cancellationToken);
        if (!isContains)
        {
            return null;
        }

        _orderingDbContext.Buyers.Update(buyer);
        await _orderingDbContext.SaveChangesAsync(cancellationToken);

        return buyer;
    }

    public async Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var buyer = await _orderingDbContext.Buyers.FindAsync(new object?[] {id}, cancellationToken);
        if (buyer is null)
        {
            return false;
        }

        _orderingDbContext.Buyers.Remove(buyer);
        var result = await _orderingDbContext.SaveChangesAsync(cancellationToken);

        return result > 0;
    }
}