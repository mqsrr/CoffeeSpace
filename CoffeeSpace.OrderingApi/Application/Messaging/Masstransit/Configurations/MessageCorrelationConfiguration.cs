using CoffeeSpace.Messages.Ordering.Commands;
using CoffeeSpace.Messages.Ordering.Responses;
using CoffeeSpace.Messages.Products.Commands;
using CoffeeSpace.Messages.Products.Responses;
using CoffeeSpace.Messages.Shipment.Commands;
using CoffeeSpace.Messages.Shipment.Responses;
using MassTransit;

namespace CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Configurations;

internal static class MessageCorrelationConfiguration
{
    public static void ConfigureStateMachineMessages()
    {
        MessageCorrelation.UseCorrelationId<SubmitOrder>(order => order.Order.Id);
        MessageCorrelation.UseCorrelationId<CancelOrder>(order => order.Order.Id);
        
        MessageCorrelation.UseCorrelationId<ValidateOrderStock>(order => order.Order.Id);
        MessageCorrelation.UseCorrelationId<OrderStockConfirmed>(order => order.Order.Id);
        MessageCorrelation.UseCorrelationId<Fault<ValidateOrderStock>>(order => order.Message.Order.Id);
        
        MessageCorrelation.UseCorrelationId<RequestOrderPayment>(order => order.Order.Id);
        MessageCorrelation.UseCorrelationId<OrderPaid>(order => order.Order.Id);
        MessageCorrelation.UseCorrelationId<Fault<RequestOrderPayment>>(order => order.Message.Order.Id);

        MessageCorrelation.UseCorrelationId<RequestOrderShipment>(order => order.Order.Id);
        MessageCorrelation.UseCorrelationId<OrderShipped>(order => order.Order.Id);
        MessageCorrelation.UseCorrelationId<Fault<RequestOrderShipment>>(order => order.Message.Order.Id);
    }
}