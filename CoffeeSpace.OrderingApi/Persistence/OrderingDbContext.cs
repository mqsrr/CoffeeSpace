using System.Reflection;
using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Persistence.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.OrderingApi.Persistence;

internal sealed class OrderingDbContext : DbContext, IOrderingDbContext
{
    public required DbSet<Order> Orders { get; init; }
    
    public required DbSet<OrderItem> OrderItems { get; init; }
    
    public required DbSet<Buyer> Buyers { get; init; }
    
    public required DbSet<Address> Addresses { get; init; }

    public OrderingDbContext(DbContextOptions<OrderingDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
    }
    
}