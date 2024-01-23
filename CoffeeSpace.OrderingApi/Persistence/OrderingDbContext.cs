using System.Reflection;
using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.OrderingApi.Persistence;

public sealed class OrderingDbContext(DbContextOptions<OrderingDbContext> options) : DbContext(options), IOrderingDbContext
{
    public required DbSet<Order> Orders { get; init; }
    
    public required DbSet<OrderItem> OrderItems { get; init; }
    
    public required DbSet<Buyer> Buyers { get; init; }
    
    public required DbSet<Address> Addresses { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
    }
    
}