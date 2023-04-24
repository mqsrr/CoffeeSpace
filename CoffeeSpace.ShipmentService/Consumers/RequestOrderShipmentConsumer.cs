using CoffeeSpace.Messages.Shipment.Events;
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

    public Task Consume(ConsumeContext<RequestOrderShipment> context)
    {
        //Transfer to local shipment service
        _logger.LogInformation($"{context.Message.Order.Id} was given to shipment service");
        Thread.Sleep(TimeSpan.FromSeconds(10));

        return context.RespondAsync<OrderShipmentResponse>(new
        {
            context.Message.Order,
            ShipmentAvailable = true
        });
    }
}