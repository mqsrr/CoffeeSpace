using CoffeeSpace.OrderingApi.Tests.Acceptance.Models;

namespace CoffeeSpace.OrderingApi.Tests.Acceptance.Helpers;

internal static class ApiEndpoints
{
    private const string ApiBase = "/api";
    
    public static class Orders
    {
        public static string Create(string buyerId) => $"{ApiBase}/buyers/{buyerId}/orders";
        public static string GetAll(string buyerId) => $"{ApiBase}/buyers/{buyerId}/orders";
        public static string Get(string buyerId, string orderId) => $"{ApiBase}/buyers/{buyerId}/orders/{orderId}";
        public static string Delete(string buyerId, string orderId) => $"{ApiBase}/buyers/{buyerId}/orders/{orderId}";
    }
    
    public static class Buyer
    {
        public static string Create() => $"{ApiBase}/buyers";
        public static string Get(string buyerId) => $"{ApiBase}/buyers/{buyerId}";
        public static string GetWithEmail(string email) => $"{ApiBase}/buyers/{email}";
        public static string Update(string buyerId) => $"{ApiBase}/buyers/{buyerId}";
        public static string Delete(string buyerId) => $"{ApiBase}/buyers/{buyerId}";
    }
}