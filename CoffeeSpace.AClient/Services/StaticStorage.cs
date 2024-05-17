using CoffeeSpace.AClient.Models;

namespace CoffeeSpace.AClient.Services;

internal static class StaticStorage
{
    public static string? JwtToken { get; set; }
    
    public static Buyer? Buyer { get; set; }
}