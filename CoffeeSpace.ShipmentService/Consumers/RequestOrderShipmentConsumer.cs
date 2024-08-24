using CoffeeSpace.Messages.Shipment.Commands;
using CoffeeSpace.Messages.Shipment.Responses;
using MassTransit;

namespace CoffeeSpace.ShipmentService.Consumers;

internal sealed class RequestOrderShipmentConsumer : IConsumer<RequestOrderShipment>
{
    private readonly ILogger<RequestOrderShipmentConsumer> _logger;

    public RequestOrderShipmentConsumer(ILogger<RequestOrderShipmentConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<RequestOrderShipment> context)
    {
        await Task.Delay(TimeSpan.FromSeconds(3));
        _logger.LogInformation("""
                               Order with ID {OrderId} was successfully transferred to the shipment service.
                               Country: {Country}
                               Street: {Street}
                               City: {City}
                               """,
            context.Message.Id,
            context.Message.Address.Country,
            context.Message.Address.Street,
            context.Message.Address.City);
        
        await context.RespondAsync<OrderShipped>(new {});
    }
}