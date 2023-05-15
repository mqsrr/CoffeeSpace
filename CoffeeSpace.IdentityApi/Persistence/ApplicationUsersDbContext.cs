using CoffeeSpace.IdentityApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.IdentityApi.Persistence;

internal sealed class ApplicationUsersDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationUsersDbContext(DbContextOptions<ApplicationUsersDbContext> options) : base(options)
    {
        
    }
}