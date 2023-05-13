﻿namespace CoffeeSpace.Domain.Ordering.Orders;

public sealed class OrderItem
{
    public string Id { get; init; } = Guid.NewGuid().ToString();
    
    public required string Title { get; init; }

    public required string Description { get; init; }

    public required float UnitPrice { get; init; }

    public required int Quantity { get; init; }

    public required float Discount { get; init; }

    public float Total => Quantity * UnitPrice;
}