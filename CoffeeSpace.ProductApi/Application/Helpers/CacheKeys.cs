namespace CoffeeSpace.ProductApi.Application.Helpers;

internal static class CacheKeys
{
    public static class Products
    {
        public const string HashKey = "products";
        
        public static string GetAll => "products-all";
        
        public static string GetById(Guid id) => $"products-{id}";
    }
}