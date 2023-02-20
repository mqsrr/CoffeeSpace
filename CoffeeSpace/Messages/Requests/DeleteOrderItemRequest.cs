using CoffeeSpace.Data.Models.Orders;
using MediatR;

namespace CoffeeSpace.Messages.Requests;

public sealed record DeleteOrderItemRequest(OrderItem Item) : IRequest<Unit>;