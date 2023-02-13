using Microsoft.AspNetCore.SignalR.Client;

namespace CoffeeSpace.Extensions;

public static class AddSignalRHubConnectionExtension
{
    public static IServiceCollection AddSignalRHubConnection(this IServiceCollection services)
    {
        services.AddSingleton(_ => new HubConnectionBuilder()
            .WithUrl("https://localhost:7194/orders")
            .WithAutomaticReconnect()
            .Build());
        
        return services;
    }
}