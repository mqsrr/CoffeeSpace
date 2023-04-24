namespace CoffeeSpace.Messages.Buyers.Commands;

public sealed record DeleteBuyer
{
    public required string Name { get; init; }

    public required string Email { get; init; }
}