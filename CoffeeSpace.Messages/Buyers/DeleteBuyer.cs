namespace CoffeeSpace.Messages.Buyers;

public sealed class DeleteBuyer
{
    public required string Name { get; init; }

    public required string Email { get; init; }
}