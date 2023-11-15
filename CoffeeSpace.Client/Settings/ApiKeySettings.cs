namespace CoffeeSpace.Client.Settings;

public sealed class ApiKeySettings
{
    public required string ApiKey { get; init; }

    public required string HeaderName { get; init; }
}