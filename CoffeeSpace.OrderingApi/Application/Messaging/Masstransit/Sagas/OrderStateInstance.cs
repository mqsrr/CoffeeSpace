using MassTransit;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Sagas;

internal sealed class OrderStateInstance : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }

    public int CurrentState { get; set; }

    public required Guid OrderId { get; set; }

    public required Guid BuyerId { get; set; }
    
    public required Guid UpdateOrderStatusCorrelationId { get; set; }
    
    public  bool StockValidationSuccess { get; set; }

    public bool PaymentSuccess { get; set; }
}