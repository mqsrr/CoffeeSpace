namespace CoffeeSpace.PaymentService.Application.Contracts.Requests;

internal sealed class GetPaypalOrderByPaypalOrderIdRequest
{
    public required string PaypalOrderId { get; init; }
}