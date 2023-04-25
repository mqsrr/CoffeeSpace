using System.Reflection;
using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.Domain.Ordering.BuyerInfo.CardInfo;
using CoffeeSpace.Domain.Ordering.CustomerInfo;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Persistence.Abstractions;
using CoffeeSpace.OrderingApi.Persistence.EntityTypeConfigurations;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.OrderingApi.Persistence;

internal sealed class OrderingDbContext : SagaDbContext, IOrderingDbContext
{
    public required DbSet<Order> Orders { get; init; }
    
    public required DbSet<OrderItem> OrderItems { get; init; }
    
    public required DbSet<Buyer> Buyers { get; init; }
    
    public required DbSet<PaymentInfo> PaymentInfos { get; init; }
    
    public required DbSet<Address> Addresses { get; init; }

    public OrderingDbContext(DbContextOptions<OrderingDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get
        {
            yield return new OrderStateMap();
        }
    }

}