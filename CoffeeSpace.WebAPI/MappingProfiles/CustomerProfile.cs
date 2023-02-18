using AutoMapper;
using CoffeeSpace.Data.Models.CustomerInfo;
using CoffeeSpace.WebAPI.Dto.Requests;

namespace CoffeeSpace.WebAPI.MappingProfiles;

public sealed class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<CustomerLoginModel, Customer>();

        CreateMap<CustomerRegisterModel, Customer>()
            .ForMember(x => x.AddressId, dest =>
                dest.MapFrom(src => src.Address.Id))
            .ForMember(x => x.PaymentId, dest =>
                dest.MapFrom(src => src.PaymentInfo.Id));
    }
}