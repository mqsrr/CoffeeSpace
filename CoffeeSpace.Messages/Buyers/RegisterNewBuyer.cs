namespace CoffeeSpace.Messages.Buyers;

public sealed class RegisterNewBuyer
{
    public required string Name { get; init; }

    public required string Email { get; init; }
}