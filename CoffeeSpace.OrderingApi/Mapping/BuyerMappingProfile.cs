using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.OrderingApi.Application.Contracts.Requests.Buyers;
using CoffeeSpace.OrderingApi.Application.Contracts.Responses.Buyers;
using Riok.Mapperly.Abstractions;

namespace CoffeeSpace.OrderingApi.Mapping;

[Mapper]
internal static partial class BuyerMappingProfile
{
    public static partial BuyerResponse ToResponse(this Buyer buyer);
    
    public static partial Buyer ToBuyer(this CreateBuyerRequest request);
    
    public static partial Buyer ToBuyer(this UpdateBuyerRequest request);
}