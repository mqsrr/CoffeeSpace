namespace CoffeeSpace.ProductApi.Tests.Acceptance.Helpers;


internal static class ApiEndpoints
{
    private const string ApiBase = "/api";

    public class Products
    {
        private const string Base = $"{ApiBase}/products";

        public const string Create = Base;
        public const string GetAll = Base;
        
        public static string Get(string productId) => $"{Base}/{productId}";
        public static string Update(string productId) => $"{Base}/{productId}";
        public static string Delete(string productId) => $"{Base}/{productId}";
    }
}