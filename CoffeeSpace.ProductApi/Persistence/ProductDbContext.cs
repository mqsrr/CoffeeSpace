using System.Reflection;
using CoffeeSpace.Domain.Products;
using CoffeeSpace.ProductApi.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.ProductApi.Persistence;

internal sealed class ProductDbContext : DbContext, IProductDbContext
{
    public required DbSet<Product> Products { get; init; }

    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}