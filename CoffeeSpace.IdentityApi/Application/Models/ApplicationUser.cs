using Microsoft.AspNetCore.Identity;

namespace CoffeeSpace.IdentityApi.Application.Models;

public sealed class ApplicationUser : IdentityUser
{
    public required string Name { get; init; }
    
    public required string LastName { get; init; }

    public required override string? UserName { get; set; }

    public required override string? Email { get; set; }

    public required string Password { get; init; }
}