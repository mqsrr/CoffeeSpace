using CoffeeSpace.IdentityApi.Application.Contracts.Requests.Register;
using CoffeeSpace.IdentityApi.Application.Models;
using Riok.Mapperly.Abstractions;

namespace CoffeeSpace.IdentityApi.Application.Mapping;

[Mapper]
public static partial class ApplicationUserMappingProfile
{
    public static partial ApplicationUser ToUser(this RegisterRequest request);
}