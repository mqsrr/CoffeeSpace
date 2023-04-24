using CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Sagas;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeeSpace.OrderingApi.Persistence.EntityTypeConfigurations;

internal sealed class OrderStateMap : SagaClassMap<OrderStateInstance>
{
    protected override void Configure(EntityTypeBuilder<OrderStateInstance> entity, ModelBuilder model)
    {
        entity.HasKey(x => x.CorrelationId);

        entity.Property(x => x.OrderId)
            .IsRequired();
        
        entity.Property(x => x.CurrentState)
            .IsRequired();

        entity.Property(x => x.PaymentSuccess)
            .IsRequired();

        entity.Property(x => x.StockValidationSuccess)
            .IsRequired();
    }
}