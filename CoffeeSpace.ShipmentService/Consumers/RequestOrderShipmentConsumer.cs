using CoffeeSpace.Messages.Shipment.Commands;
using CoffeeSpace.Messages.Shipment.Responses;
using MassTransit;

namespace CoffeeSpace.ShipmentService.Consumers;

internal sealed class RequestOrderShipmentConsumer : IConsumer<RequestOrderShipment>
{
    private readonly ILogger<RequestOrderShipmentConsumer> _logger;
    private readonly ITopicProducer<OrderShipped> _topicProducer;

    public RequestOrderShipmentConsumer(ILogger<RequestOrderShipmentConsumer> logger, ITopicProducer<OrderShipped> topicProducer)
    {
        _logger = logger;
        _topicProducer = topicProducer;
    }

    public async Task Consume(ConsumeContext<RequestOrderShipment> context)
    {
        //Transfer to local shipment service
        await Task.Delay(TimeSpan.FromSeconds(3));
        
        _logger.LogInformation("Order with ID {OrderId} was successfully transferred to the shipment service", context.Message.Order.Id);
        await _topicProducer.Produce(new
        {
            context.Message.Order,
            ShipmentAvailable = true
        });
    }
}