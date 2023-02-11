using CoffeeSpace.Data.Models.Orders;
using MediatR;

namespace CoffeeSpace.Messages.Requests;

public sealed class DeleteOrderItemRequest : IRequest
{
    public OrderItem Item { get; }
    
}