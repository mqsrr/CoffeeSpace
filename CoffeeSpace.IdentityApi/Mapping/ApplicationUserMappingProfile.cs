using CoffeeSpace.IdentityApi.Contracts.Requests.Register;
using CoffeeSpace.IdentityApi.Models;
using Riok.Mapperly.Abstractions;

namespace CoffeeSpace.IdentityApi.Mapping;

[Mapper]
public static partial class ApplicationUserMappingProfile
{
    public static partial ApplicationUser ToUser(this RegisterRequest request);
}