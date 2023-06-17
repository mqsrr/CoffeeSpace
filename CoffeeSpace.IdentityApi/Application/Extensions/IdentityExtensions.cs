using CoffeeSpace.IdentityApi.Application.Models;
using CoffeeSpace.IdentityApi.Persistence;
using Microsoft.AspNetCore.Identity;

namespace CoffeeSpace.IdentityApi.Application.Extensions;

internal static class IdentityExtensions
{
    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services,
        PasswordOptions? passwordOptions = null, UserOptions? userOptions = null)
    {
        passwordOptions ??= new PasswordOptions
        {
            RequireDigit = true,
            RequiredLength = 6,
            RequireLowercase = true,
            RequireUppercase = true,
            RequireNonAlphanumeric = true
        };

        userOptions ??= new UserOptions
        {
            RequireUniqueEmail = true
        };

        services.AddIdentity<ApplicationUser, IdentityRole>(x =>
            {
                x.Password = passwordOptions;
                x.User = userOptions;
            })
            .AddEntityFrameworkStores<ApplicationUsersDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}