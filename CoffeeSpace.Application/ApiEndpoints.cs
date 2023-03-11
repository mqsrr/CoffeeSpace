namespace CoffeeSpace.Application;

public static class ApiEndpoints
{
    private const string ApiBase = "api";

    public static class OrderItems
    {
        private const string Base = $"{ApiBase}/orderitems";

        public const string Create = Base;
        public const string GetAll = Base;
        public const string Get = $"{Base}/{{id:guid}}";
        public const string Update = Base;
        public const string Delete = $"{Base}/{{id:guid}}";
    }
    
    public static class Orders
    {
        private const string Base = $"{ApiBase}/orders";

        public const string Create = Base;
        public const string GetAll = Base;
        public const string Get = $"{Base}/{{id:guid}}";
        public const string Update = Base;
        public const string Delete = $"{Base}/{{id:guid}}";
    }
    
    public static class Customers
    {
        private const string Base = $"{ApiBase}/customers";

        public const string Create = Base;
        public const string Get = $"{Base}/{{id:guid}}";
        public const string GetOrders = $"{Base}/{{id:guid}}/orders";
        public const string Update = Base;
        public const string Delete = $"{Base}/{{id:guid}}";
    }
    
    public static class Auth
    {
        private const string Base = $"{ApiBase}/auth";

        public const string Login = $"{Base}/login";
        public const string Register = $"{Base}/Register";
    }

}