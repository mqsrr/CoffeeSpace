namespace CoffeeSpace.Core.Settings;

public sealed class AwsMessagingSettings
{
    public required string Region { get; init; }

    public required string AccessKey { get; init; }

    public required string SecretKey { get; init; }
}