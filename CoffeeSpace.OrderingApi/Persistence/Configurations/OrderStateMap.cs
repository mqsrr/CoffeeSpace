using CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Sagas;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoffeeSpace.OrderingApi.Persistence.Configurations;

internal sealed class OrderStateMap : SagaClassMap<OrderStateInstance>
{
    protected override void Configure(EntityTypeBuilder<OrderStateInstance> entity, ModelBuilder model)
    {
        entity.Property(x => x.CurrentState)
            .IsRequired()
            .IsUnicode(false);

        entity.Property(x => x.PaymentSuccess)
            .IsRequired();

        entity.Property(x => x.StockValidationSuccess)
            .IsRequired();
        
        entity.HasIndex(x => x.CorrelationId)
            .IsUnique();

    }
}