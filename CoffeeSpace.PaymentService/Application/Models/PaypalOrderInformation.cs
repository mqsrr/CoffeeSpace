using PayPalCheckoutSdk.Orders;

namespace CoffeeSpace.PaymentService.Application.Models;

public sealed class PaypalOrderInformation
{
    public required string Id { get; init; }
    
    public required string ApplicationOrderId { get; init; }

    public required string PaypalOrderId { get; init; }

    public required string BuyerId { get; init; }

    public required Order PaypalOrder { get; init; }
}