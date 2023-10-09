namespace CoffeeSpace.PaymentService.Contracts.Requests;

internal sealed class GetPaypalOrderByPaypalOrderIdRequest
{
    public required string PaypalOrderId { get; init; }
}