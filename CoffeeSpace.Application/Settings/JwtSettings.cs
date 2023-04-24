namespace CoffeeSpace.Application.Settings;

public sealed class JwtSettings
{
    public required string Audience { get; init; }
    
    public required string Issuer { get; init; }
    
    public required string Key { get; init; }
    
    public required int Expire { get; init; }
}