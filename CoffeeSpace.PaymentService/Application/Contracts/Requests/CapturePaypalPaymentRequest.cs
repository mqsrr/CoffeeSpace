namespace CoffeeSpace.PaymentService.Application.Contracts.Requests;

internal sealed class CapturePaypalPaymentRequest
{
    public required string Token { get; init; }
}