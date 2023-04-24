namespace CoffeeSpace.Application.Settings;

public sealed class RabbitMqSettings
{
    public required string Host { get; init; }

    public required string Username { get; init; }
    
    public required string Password { get; init; }
}