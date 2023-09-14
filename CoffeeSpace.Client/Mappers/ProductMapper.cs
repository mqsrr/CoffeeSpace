using CoffeeSpace.Client.Models.Ordering;
using CoffeeSpace.Client.Models.Products;
using Riok.Mapperly.Abstractions;

namespace CoffeeSpace.Client.Mappers;

[Mapper]
public static partial class ProductMapper
{
    public static partial OrderItem ToOrderItem(this Product product);
}
