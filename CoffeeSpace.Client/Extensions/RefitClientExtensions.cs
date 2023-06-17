using CoffeeSpace.Client.Handlers;
using Refit;

namespace CoffeeSpace.Client.Extensions;

public static class RefitClientExtensions
{
    private static IHttpClientBuilder AddUnauthorizedApiClient<TClient>(this IServiceCollection services)
        where TClient : class
    {
        return services.AddRefitClient<TClient>()
            .ConfigureHttpClient(config => config.BaseAddress = new Uri("http://localhost:8085"));
    }
    
    public static IServiceCollection AddWebApiClient<TClient>(this IServiceCollection services, bool requiresAuthorization)
        where TClient : class
    {
        var httpClientBuilder = services.AddUnauthorizedApiClient<TClient>();
        if (requiresAuthorization)
        {
            httpClientBuilder.AddHttpMessageHandler<AuthHeaderHandler>();
        }
        
        return services;
    }
}