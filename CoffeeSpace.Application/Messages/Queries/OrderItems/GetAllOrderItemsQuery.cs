using CoffeeSpace.Domain.Models.Orders;
using Mediator;

namespace CoffeeSpace.Application.Messages.Queries.OrderItems;

public sealed class GetAllOrderItemsQuery : IQuery<IEnumerable<OrderItem>>
{
}