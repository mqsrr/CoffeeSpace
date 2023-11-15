using CoffeeSpace.PaymentService.Models;
using Order = CoffeeSpace.Domain.Ordering.Orders.Order;

namespace CoffeeSpace.PaymentService.Services.Abstractions;

public interface IPaymentService
{
    Task<PaypalOrderInformation?> GetOrderPaymentByOrderIdAsync(string paypalOrderId, CancellationToken cancellationToken);
    
    Task<PayPalCheckoutSdk.Orders.Order?> CreateOrderAsync(Order order, CancellationToken cancellationToken);

    Task<PayPalCheckoutSdk.Orders.Order?> CapturePaypalPaymentAsync(string paypalPaymentId, CancellationToken cancellationToken);

}