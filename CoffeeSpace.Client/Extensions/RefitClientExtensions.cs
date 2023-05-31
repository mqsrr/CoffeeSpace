using CoffeeSpace.Client.Handlers;
using Refit;

namespace CoffeeSpace.Client.Extensions;

public static class RefitClientExtensions
{
    public static IServiceCollection AddWebApiClient<TClient>(this IServiceCollection services)
        where TClient : class
    {
        services.AddRefitClient<TClient>()
            .ConfigureHttpClient(config => config.BaseAddress = new Uri("http://localhost:8085"));

        return services;
    }
    
    public static IServiceCollection AddAuthenticationWebApiClient<TClient>(this IServiceCollection services)
        where TClient : class
    {
        services.AddRefitClient<TClient>()
            .ConfigureHttpClient(config => config.BaseAddress = new Uri("http://localhost:8085"))
            .AddHttpMessageHandler<AuthHeaderHandler>();

        return services;
    }
}