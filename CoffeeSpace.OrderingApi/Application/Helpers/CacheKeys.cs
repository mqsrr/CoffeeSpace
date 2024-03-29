namespace CoffeeSpace.OrderingApi.Application.Helpers;

internal static class CacheKeys
{
    public static class Buyers
    {
        public const string HashKey = "buyers";   
        public static string Get(Guid id) => $"buyers-id-{id}";
        public static string GetByEmail(string email) => $"buyers-email-{email}";
    }   
    
    public static class Order
    {
        public const string HashKey = "orders";   
        public static string GetAll(Guid customerId) => $"orders-{customerId}-all";
        public static string GetByCustomerId(Guid id, Guid customerId) => $"orders-{customerId}-id-{id}";
    }
}