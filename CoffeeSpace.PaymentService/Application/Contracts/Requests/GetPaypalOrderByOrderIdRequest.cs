namespace CoffeeSpace.PaymentService.Application.Contracts.Requests;

internal sealed class GetPaypalOrderByOrderIdRequest
{
    public required Guid OrderId { get; init; }
}