using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Orders;
using CoffeeSpace.OrderingApi.Application.Contracts.Responses.Orders;
using Riok.Mapperly.Abstractions;

namespace CoffeeSpace.OrderingApi.Application.Mapping;

[Mapper]
internal static partial class OrderMappingProfile
{
    public static partial OrderResponse ToResponse(this Order order);
    
    private static partial Order ToOrder(this CreateOrderRequest request);
    
    public static Order ToOrder(this CreateOrderRequest request, Guid buyerId)
    {
        request.BuyerId = buyerId;
        
        return request.ToOrder();
    }
    
    private static partial Order ToOrder(this UpdateOrderRequest request);
    
    public static Order ToOrder(this UpdateOrderRequest request, Guid id, Guid buyerId)
    {
        request.Id = id;
        request.BuyerId = buyerId;
        
        return request.ToOrder();
    }
}