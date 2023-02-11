using IdentityModel.OidcClient;
using MauiAuth0App.Auth0;

namespace CoffeeSpace.Extensions;

public static class AddOidClientExtension
{
    public static IServiceCollection AddOidClient(this IServiceCollection services, OidcClient client = null)
    {
        client ??= new OidcClient(new OidcClientOptions
        {
            Authority = "https://demo.duendesoftware.com",

            ClientId = "interactive.public",
            Scope = "openid profile api",
            RedirectUri = "myapp://callback",

            Browser = new WebBrowserAuthenticator()
        });
        
        services.AddSingleton(client);
        
        return services;
    }
}