using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Orders;
using CoffeeSpace.OrderingApi.Application.Contracts.Responses.Orders;
using Riok.Mapperly.Abstractions;

namespace CoffeeSpace.OrderingApi.Mapping;

[Mapper]
internal static partial class OrderMappingProfile
{
    public static partial OrderResponse ToResponse(this Order order);
    
    public static partial Order ToOrder(this CreateOrderRequest request);
    
    public static partial Order ToOrder(this UpdateOrderRequest request);
}