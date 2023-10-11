namespace CoffeeSpace.IdentityApi.Settings;

internal sealed class ApiKeySettings
{
    public const string SectionName = "Authorization";
    
    public required string ApiKey { get; init; }

    public required string HeaderName { get; init; }
}