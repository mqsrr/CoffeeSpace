using CoffeeSpace.Data.Models.CustomerInfo;
using CoffeeSpace.Data.Models.Orders;
using MediatR;

namespace CoffeeSpace.Messages.Requests;

public sealed record CreateOrderRequest(IEnumerable<OrderItem> OrderItems, Customer Customer) : IRequest<Unit>;