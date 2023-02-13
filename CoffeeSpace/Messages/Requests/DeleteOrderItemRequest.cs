using CoffeeSpace.Data.Models.Orders;
using MediatR;

namespace CoffeeSpace.Messages.Requests;

public sealed class DeleteOrderItemRequest : IRequest<Unit>
{
    public OrderItem Item { get; }
    
}