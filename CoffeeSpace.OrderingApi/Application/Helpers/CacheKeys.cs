namespace CoffeeSpace.OrderingApi.Application.Helpers;

internal static class CacheKeys
{
    public static class Buyers
    {
        public static string Get(string id) => $"buyers-{id}";
        public static string GetByEmail(string email) => $"buyers-email-{email}";
    }   
    
    public static class Order
    {
        public static string GetAll(string customerId) => $"orders-{customerId}-all";
        public static string GetByCustomerId(string id, string customerId) => $"orders-{customerId}-{id}";
    }
}