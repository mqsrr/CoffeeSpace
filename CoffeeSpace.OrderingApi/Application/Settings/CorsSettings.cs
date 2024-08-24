namespace CoffeeSpace.OrderingApi.Application.Settings;

internal sealed class CorsSettings
{
    public  static readonly string[] AllowedOrigins = {"http://localhost:4200"};
    
    public  static readonly string[] AllowedMethods = {"POST"};

    public static readonly string[] AllowedHeaders = {"x-requested-with", "x-signalr-user-agent"};
}