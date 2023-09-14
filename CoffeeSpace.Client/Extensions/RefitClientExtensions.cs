using CoffeeSpace.Client.Handlers;
using Refit;

namespace CoffeeSpace.Client.Extensions;

public static class RefitClientExtensions
{
    public static IServiceCollection AddWebApiClient<TClient>(this IServiceCollection services, bool requiresAuthorization)
        where TClient : class
    {
        string baseAddress = DeviceInfo.Current.Platform == DevicePlatform.Android
            ? "http://10.0.2.2:8085"
            : "http://localhost:8085";

        var httpClientBuilder = services.AddRefitClient<TClient>().ConfigureHttpClient(config =>
            config.BaseAddress = new Uri(baseAddress));

        if (requiresAuthorization)
        {
            httpClientBuilder.AddHttpMessageHandler<AuthHeaderHandler>();
        }
        return services;
    }
}