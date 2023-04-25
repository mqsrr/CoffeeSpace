using CoffeeSpace.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.ProductApi.Persistence;

internal interface IProductDbContext
{
    public DbSet<Product> Products { get; init; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}