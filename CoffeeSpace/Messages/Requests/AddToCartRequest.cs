using CoffeeSpace.Data.Models.Orders;
using MediatR;

namespace CoffeeSpace.Messages.Requests;

public sealed class AddToCartRequest : IRequest<Unit>
{
    public OrderItem Item { get; }

    public AddToCartRequest(OrderItem item) => Item = item;
}