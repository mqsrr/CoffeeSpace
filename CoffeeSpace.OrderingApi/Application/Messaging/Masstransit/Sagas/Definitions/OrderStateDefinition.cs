using System.Data.Common;
using CoffeeSpace.OrderingApi.Persistence;
using MassTransit;
using MassTransit.Configuration;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Sagas.Definitions;

internal sealed class OrderStateDefinition : SagaDefinition<OrderStateInstance>
{
    protected override void ConfigureSaga(
        IReceiveEndpointConfigurator endpointConfigurator, 
        ISagaConfigurator<OrderStateInstance> sagaConfigurator,
        IRegistrationContext registrationContext)
    {
        endpointConfigurator.UseEntityFrameworkOutbox<OrderStateSagaDbContext>(registrationContext);
    }
}