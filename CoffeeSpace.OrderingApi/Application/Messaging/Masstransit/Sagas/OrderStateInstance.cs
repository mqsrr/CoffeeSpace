using MassTransit;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Sagas;

internal sealed class OrderStateInstance : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }

    public int CurrentState { get; set; }

    public string? OrderId { get; set; }
    
    public string? BuyerId { get; set; }
    
    public string? UpdateOrderStatudCorrelationId { get; set; }
    
    public bool StockValidationSuccess { get; set; }

    public bool PaymentSuccess { get; set; }
}