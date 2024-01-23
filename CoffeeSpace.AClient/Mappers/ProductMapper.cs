using CoffeeSpace.AClient.Models;
using Riok.Mapperly.Abstractions;

namespace CoffeeSpace.AClient.Mappers;

[Mapper]
internal static partial class ProductMapper
{
    public static partial OrderItem ToOrderItem(this Product product);
}