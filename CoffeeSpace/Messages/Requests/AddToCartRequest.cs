using CoffeeSpace.Data.Models.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CoffeeSpace.Messages.Requests;

public sealed class AddToCartRequest : IRequest
{
    public OrderItem Item { get; }

    public AddToCartRequest(OrderItem item) => Item = item;
}