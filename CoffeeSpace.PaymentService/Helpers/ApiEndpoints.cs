namespace CoffeeSpace.PaymentService.Helpers;

internal sealed class ApiEndpoints
{
    private const string ApiBase = "/api";
    
    public static class Payments
    {
        public const string GetByApplicationOrderId = $"{ApiBase}/application-orders/{{orderId:guid}}";
        public const string CapturePaypalPayment = "/";
    }
}