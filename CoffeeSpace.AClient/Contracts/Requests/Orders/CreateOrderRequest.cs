using System.Collections.Generic;

namespace CoffeeSpace.AClient.Contracts.Requests.Orders;

public sealed class CreateOrderRequest
{
    public required int Status { get; init; }
    
    public required CreateAddressRequest Address { get; init; }
    
    public required IEnumerable<CreateOrderItemRequest> OrderItems { get; init; }
}