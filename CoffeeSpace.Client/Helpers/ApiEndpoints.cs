namespace CoffeeSpace.Client.Helpers;

public static class ApiEndpoints
{
    public static class Authentication
    {
        private const string Base = "/auth";

        public const string Register = $"{Base}/register";
        public const string Login = $"{Base}/login";
    }

    public static class Products
    {
        private const string Base = "/products";

        public const string GetAll = Base;
        public const string GetById = $"{Base}/{{id}}";
    }

    public static class Buyer
    {
        private const string Base = "/buyers";

        public const string GetById = $"{Base}/{{id}}";
        public const string GetByEmail = $"{Base}/{{email}}";
    }
    
    public static class Orders
    {
        private const string Base = $"/buyers/{{buyerId}}/orders";

        public const string Create = Base;
        public const string GetAll = Base;
        public const string Get = $"{Base}/{{id}}";
        public const string Delete = $"{Base}/{{id}}";
    }
}