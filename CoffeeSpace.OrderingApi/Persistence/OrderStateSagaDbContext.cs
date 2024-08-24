using CoffeeSpace.OrderingApi.Persistence.Configurations;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

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
}