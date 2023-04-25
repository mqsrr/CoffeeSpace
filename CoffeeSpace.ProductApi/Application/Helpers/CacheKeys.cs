namespace CoffeeSpace.ProductApi.Application.Helpers;

internal static class CacheKeys
{
    public static class Products
    {
        public static string GetAll => "products-all";
        
        public static string GetById(string id) => $"products-{id}";
    }
}