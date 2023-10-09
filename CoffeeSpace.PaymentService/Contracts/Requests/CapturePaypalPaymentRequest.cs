namespace CoffeeSpace.PaymentService.Contracts.Requests;

internal sealed class CapturePaypalPaymentRequest
{
    public required string Token { get; init; }
}