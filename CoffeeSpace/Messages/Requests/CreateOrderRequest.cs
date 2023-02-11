using CoffeeSpace.Data.Models.CustomerInfo;
using CoffeeSpace.Data.Models.Orders;
using MediatR;

namespace CoffeeSpace.Messages.Requests;

public sealed class CreateOrderRequest : IRequest
{
    public IEnumerable<OrderItem> OrderItems { get; }
    public Customer Customer { get; }

    public CreateOrderRequest(IEnumerable<OrderItem> orderItems, Customer customer)
    {
        OrderItems = orderItems;
        Customer = customer;
    }
}