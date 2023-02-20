using CoffeeSpace.Data.Models.Orders;
using MediatR;

namespace CoffeeSpace.Messages.Requests;

public sealed record AddToCartRequest(OrderItem Item) : IRequest<Unit>;