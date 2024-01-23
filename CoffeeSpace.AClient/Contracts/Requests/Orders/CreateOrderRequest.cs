using System.Collections.Generic;
using CoffeeSpace.AClient.Models;

namespace CoffeeSpace.AClient.Contracts.Requests.Orders;

public sealed class CreateOrderRequest
{
    public required int Status { get; init; }
    
    public required CreateAddressRequest Address { get; init; }
    
    public required IEnumerable<OrderItem> OrderItems { get; init; }
}