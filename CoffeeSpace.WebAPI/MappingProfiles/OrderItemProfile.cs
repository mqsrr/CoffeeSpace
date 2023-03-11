using AutoMapper;
using CoffeeSpace.Application.Models.Orders;
using CoffeeSpace.Contracts.Requests.OrderItem;
using CoffeeSpace.Contracts.Responses.OrderItems;

namespace CoffeeSpace.WebAPI.MappingProfiles;

public sealed class OrderItemProfile : Profile
{
    public OrderItemProfile()
    {
        CreateMap<CreateOrderItemRequest, OrderItem>();
        
        CreateMap<UpdateOrderItemRequest, OrderItem>();
        
        CreateMap<OrderItem, OrderItemResponse>();
        
        CreateMap<IEnumerable<OrderItem>, OrderItemsResponse>()
            .ForMember(x => x.OrderItemResponses,
                dest => dest.MapFrom(src => src));
    }
}