namespace CoffeeSpace.Shared.Settings;

public sealed class AwsMessagingSettings
{
    public const string SectionName = "AWS";
    
    public required string Region { get; init; }

    public required string AccessKey { get; init; }

    public required string SecretKey { get; init; }
}