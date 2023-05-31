using CoffeeSpace.Client.Contracts.Products;
using CoffeeSpace.Domain.Ordering.Orders;
using Riok.Mapperly.Abstractions;

namespace CoffeeSpace.Client.Mapping;

[Mapper]
public static partial class OrderItemMappingConfiguration
{
    public static partial OrderItem ToOrderItem(this ProductResponse product);
}