using CoffeeSpace.Application.Models.CustomerInfo;
using CoffeeSpace.Application.Models.Orders;
using MediatR;

namespace CoffeeSpace.Messages.Requests;

public sealed record CreateOrderRequest(IEnumerable<OrderItem> OrderItems, Customer Customer) : IRequest<Unit>;