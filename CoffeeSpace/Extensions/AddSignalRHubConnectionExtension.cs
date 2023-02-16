using Microsoft.AspNetCore.SignalR.Client;

namespace CoffeeSpace.Extensions;

public static class AddSignalRHubConnectionExtension
{
    public static IServiceCollection AddSignalRHubConnection(this IServiceCollection services)
    {
        string baseUrl = DeviceInfo.Platform == DevicePlatform.Android
            ? "http://10.0.2.2:5109/orders"
            : "https://localhost:7194/orders";
        
        services.AddSingleton(_ => new HubConnectionBuilder()
            .WithUrl(baseUrl)
            .WithAutomaticReconnect()
            .Build());
        
        return services;
    }
}