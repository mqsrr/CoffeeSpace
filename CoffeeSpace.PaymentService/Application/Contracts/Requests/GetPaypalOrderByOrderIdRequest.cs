namespace CoffeeSpace.PaymentService.Application.Contracts.Requests;

internal sealed class GetPaypalOrderByOrderIdRequest
{
    public required string OrderId { get; init; }
}