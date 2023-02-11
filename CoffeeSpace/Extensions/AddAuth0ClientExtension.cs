using CoffeeSpace.Auth0;

namespace CoffeeSpace.Extensions;

public static class AddAuth0ClientExtension
{
    public static IServiceCollection AddAuth0Client(this IServiceCollection services, Auth0Client client = null)
    {
        client ??= new Auth0Client(new Auth0ClientOptions
        {
            Domain = "dev-na4wzbup5hqty85k.us.auth0.com",
            ClientId = "Mt9p4yjBNrISpBI5oGde1GRriz804S5L",
            Scope = "openid profile",

            RedirectUri = "myapp://callback"
        });
        
         services.AddSingleton(client);

         return services;
    }
}