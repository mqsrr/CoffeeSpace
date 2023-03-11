using AutoMapper;
using CoffeeSpace.Application.Models.CustomerInfo;
using CoffeeSpace.Contracts.Requests.Customer;
using CoffeeSpace.Contracts.Responses.Customers;

namespace CoffeeSpace.WebAPI.MappingProfiles;

public sealed class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<LoginRequest, Customer>();

        CreateMap<RegisterRequest, Customer>()
            .ForMember(x => x.AddressId, dest =>
                dest.MapFrom(src => src.Address.Id))
            .ForMember(x => x.PaymentId, dest =>
                dest.MapFrom(src => src.PaymentInfo.Id));

        CreateMap<CreateCustomerRequest, Customer>()
            .ForMember(x => x.AddressId, dest =>
                dest.MapFrom(src => src.Address.Id))
            .ForMember(x => x.PaymentId, dest =>
                dest.MapFrom(src => src.PaymentInfo.Id));

        CreateMap<UpdateCustomerRequest, Customer>()
            .ForMember(x => x.AddressId, dest =>
                dest.MapFrom(src => src.Address.Id))
            .ForMember(x => x.PaymentId, dest =>
                dest.MapFrom(src => src.PaymentInfo.Id));

        CreateMap<Customer, CustomerResponse>();
    }
}