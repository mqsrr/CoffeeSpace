using CoffeeSpace.Domain.Ordering.Orders;
using MassTransit;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Sagas;

internal sealed class OrderStateInstance : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }

    public int CurrentState { get; set; }
    
    public required Order Order { get; set; }
    
    public  bool StockValidationSuccess { get; set; }

    public bool PaymentSuccess { get; set; }
}