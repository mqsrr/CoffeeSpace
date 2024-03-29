using PayPalCheckoutSdk.Orders;

namespace CoffeeSpace.PaymentService.Application.Models;

public sealed class PaypalOrderInformation
{
    public required Guid Id { get; init; }
    
    public required Guid ApplicationOrderId { get; init; }

    public required string PaypalOrderId { get; init; }

    public required Guid BuyerId { get; init; }

    public required Order PaypalOrder { get; init; }
}