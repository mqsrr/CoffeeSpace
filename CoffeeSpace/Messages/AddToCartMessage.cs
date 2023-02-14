using CoffeeSpace.Data.Models.Orders;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace CoffeeSpace.Messages;

public class AddToCartMessage : ValueChangedMessage<OrderItem>
{
    public AddToCartMessage(OrderItem value) : base(value)
    {
    }
}
