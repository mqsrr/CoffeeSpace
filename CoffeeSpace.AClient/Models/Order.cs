using System.Collections.Generic;

namespace CoffeeSpace.AClient.Models;

public sealed class Order
{
    public required string Id { get; init; }
    
    public required OrderStatus Status { get; set; }

    public required string BuyerId { get; init; }

    public required Address Address { get; init; }
    
    public required IEnumerable<OrderItem> OrderItems { get; init; }
}