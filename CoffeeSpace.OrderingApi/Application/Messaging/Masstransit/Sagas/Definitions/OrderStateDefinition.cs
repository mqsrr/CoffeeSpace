using CoffeeSpace.OrderingApi.Persistence;
using MassTransit;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Sagas.Definitions;

internal sealed class OrderStateDefinition : SagaDefinition<OrderStateInstance>
{
    protected override void ConfigureSaga(
        IReceiveEndpointConfigurator endpointConfigurator, 
        ISagaConfigurator<OrderStateInstance> sagaConfigurator,
        IRegistrationContext registrationContext)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(10, 50, 100, 1000, 1000, 1000, 1000, 1000));
        endpointConfigurator.UseEntityFrameworkOutbox<OrderStateSagaDbContext>(registrationContext);
    }
}