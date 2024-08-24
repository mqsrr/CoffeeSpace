using CoffeeSpace.AClient.Contracts.Requests.Orders;
using CoffeeSpace.AClient.Models;
using Riok.Mapperly.Abstractions;

namespace CoffeeSpace.AClient.Mappers;

[Mapper]
internal static partial class ProductMapper
{
    public static partial CreateOrderItemRequest ToOrderItemRequest(this Product product);
}