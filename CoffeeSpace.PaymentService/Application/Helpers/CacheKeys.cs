namespace CoffeeSpace.PaymentService.Application.Helpers;

internal sealed class CacheKeys
{
    public static class Payments
    {
        public const string HashKey = "payments";

        public static string GetById(string id) => $"payment-id-{id}";
    }
}