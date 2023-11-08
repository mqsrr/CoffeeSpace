using CoffeeSpace.Client.Models.Ordering;
using CoffeeSpace.Client.Models.Products;
using Riok.Mapperly.Abstractions;

namespace CoffeeSpace.Client.Mappers;

[Mapper]
public static partial class ProductMapper
{
   private static partial OrderItem MapToOrderItem(this Product product);

    public static OrderItem ToOrderItem(this Product product)
    {
        var orderItem = product.MapToOrderItem();
        orderItem!.Quantity = product.Quantity;

        return orderItem;
    }
}
