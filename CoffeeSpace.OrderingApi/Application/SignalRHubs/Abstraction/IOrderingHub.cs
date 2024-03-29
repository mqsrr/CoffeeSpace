using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Contracts.Responses.Orders;

namespace CoffeeSpace.OrderingApi.Application.SignalRHubs.Abstraction;

public interface IOrderingHub
{
    Task OrderCreated(OrderResponse createdOrder);

    Task OrderStatusUpdated(OrderStatus newOrderStatus, Guid orderId);
    
    Task OrderPaymentPageInitialized(Guid orderId, string paymentApprovalLink);
}