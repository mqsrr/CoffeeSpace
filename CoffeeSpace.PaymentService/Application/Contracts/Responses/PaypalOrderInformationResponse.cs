using PayPalCheckoutSdk.Orders;

namespace CoffeeSpace.PaymentService.Application.Contracts.Responses;

internal sealed class PaypalOrderInformationResponse
{
    public required Guid Id { get; init; }
    
    public required Guid ApplicationOrderId { get; init; }

    public required string PaypalOrderId { get; init; }
    
    public required Order PaypalOrder { get; init; }
}