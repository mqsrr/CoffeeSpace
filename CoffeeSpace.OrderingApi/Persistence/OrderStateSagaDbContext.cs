using CoffeeSpace.OrderingApi.Persistence.Configurations;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Quartz.Spi;

namespace CoffeeSpace.OrderingApi.Persistence;

internal sealed class OrderStateSagaDbContext : SagaDbContext
{
    public OrderStateSagaDbContext(DbContextOptions<OrderStateSagaDbContext> options) : base(options)
    {
    }

    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get
        {
            yield return new OrderStateMap();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
         modelBuilder.AddInboxStateEntity();
         modelBuilder.AddOutboxMessageEntity();
         modelBuilder.AddOutboxStateEntity();
         
         base.OnModelCreating(modelBuilder);
    }
}