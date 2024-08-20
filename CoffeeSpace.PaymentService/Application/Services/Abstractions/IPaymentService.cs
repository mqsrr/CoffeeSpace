using CoffeeSpace.PaymentService.Models;
using Order = CoffeeSpace.Domain.Ordering.Orders.Order;

namespace CoffeeSpace.PaymentService.Application.Services.Abstractions;

public interface IPaymentService
{
    Task<PayPalCheckoutSdk.Orders.Order?> CreateOrderAsync(Order order, CancellationToken cancellationToken);

    Task CapturePaypalPaymentAsync(OrderApprovedWebhookEvent webhookEvent, CancellationToken cancellationToken);

}