namespace CoffeeSpace.Shared.Settings;

public sealed class KafkaSettings
{
    public const string SectionName = "Kafka";
    
    public required IReadOnlyList<string> Hosts { get; set; }
    
    public required string Username { get; init; }
    
    public required string Password { get; init; }
}