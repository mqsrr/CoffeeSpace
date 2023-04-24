namespace CoffeeSpace.Messages.Buyers.Commands;

public record RegisterNewBuyer
{
    public required string Name { get; init; }

    public required string Email { get; init; }
}