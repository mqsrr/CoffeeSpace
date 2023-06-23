using CoffeeSpace.OrderingApi.Persistence;
using MassTransit;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Sagas.Definitions;

internal sealed class OrderStateDefinition : SagaDefinition<OrderStateInstance>
{
    private readonly IServiceProvider _provider;
    
    public OrderStateDefinition(IServiceProvider provider)
    {
        _provider = provider;
    }
    
    protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, 
        ISagaConfigurator<OrderStateInstance> sagaConfigurator)
    {
        endpointConfigurator.UseEntityFrameworkOutbox<OrderStateSagaDbContext>(_provider);
    }
}