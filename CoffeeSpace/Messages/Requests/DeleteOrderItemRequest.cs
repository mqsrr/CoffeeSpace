using CoffeeSpace.Application.Models.Orders;
using MediatR;

namespace CoffeeSpace.Messages.Requests;

public sealed record DeleteOrderItemRequest(OrderItem Item) : IRequest<Unit>;