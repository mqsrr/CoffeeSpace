using System.Reflection;
using CoffeeSpace.Domain.Products;
using CoffeeSpace.ProductApi.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.ProductApi.Persistence;

internal sealed class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options), IProductDbContext
{
    public required DbSet<Product> Products { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}