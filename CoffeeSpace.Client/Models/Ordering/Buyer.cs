namespace CoffeeSpace.Client.Models.Ordering;

public sealed class Buyer
{
    public required string Id { get; init; }  
    
    public required string Name { get; init; }
    
    public required string Email { get; init; }

    public IEnumerable<Order> Orders { get; init; }
}