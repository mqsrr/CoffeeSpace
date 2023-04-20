using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.Domain.Products;
using Riok.Mapperly.Abstractions;

namespace CoffeeSpace.OrderingApi.Mapping;

[Mapper]
internal static partial class OrderItemMappingProfile
{
    public static partial Product ToProduct(this OrderItem orderItem);
}