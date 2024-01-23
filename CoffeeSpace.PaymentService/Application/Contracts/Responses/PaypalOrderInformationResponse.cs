using PayPalCheckoutSdk.Orders;

namespace CoffeeSpace.PaymentService.Application.Contracts.Responses;

internal sealed class PaypalOrderInformationResponse
{
    public required string Id { get; init; }
    
    public required string ApplicationOrderId { get; init; }

    public required string PaypalOrderId { get; init; }
    
    public required Order PaypalOrder { get; init; }
}