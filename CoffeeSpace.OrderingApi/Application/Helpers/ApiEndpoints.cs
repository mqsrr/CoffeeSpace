namespace CoffeeSpace.OrderingApi.Application.Helpers;

internal static class ApiEndpoints
{
    private const string ApiBase = "/api";
    
    public static class Orders
    {
        private const string Base = $"{ApiBase}/buyers/{{buyerId:guid}}/orders";

        public const string Create = Base;
        public const string GetAll = Base;
        public const string Get = $"{Base}/{{id:guid}}";
        public const string Update = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}";
    }
    
    public static class Buyer
    {
        private const string Base = $"{ApiBase}/buyers";

        public const string Create = Base;
        public const string Get = $"{Base}/{{id:guid}}";
        public const string GetWithEmail = $"{Base}/{{email}}";
        public const string Update = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}";
    }
}