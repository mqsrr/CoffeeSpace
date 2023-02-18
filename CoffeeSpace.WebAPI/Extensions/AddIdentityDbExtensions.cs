using CoffeeSpace.Data.Context;
using CoffeeSpace.Data.Models.CustomerInfo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

namespace CoffeeSpace.WebAPI.Extensions;

public static class AddIdentityDbExtensions
{
    public static IServiceCollection AddIdentityDb(this IServiceCollection services, PasswordOptions? passwordOptions = null!)
    {
        passwordOptions ??= new PasswordOptions
        {
            RequireDigit = true,
            RequiredLength = 6,
            RequireLowercase = true,
            RequireUppercase = true,
            RequireNonAlphanumeric = false
        };
        
        services.AddIdentity<Customer, IdentityRole>(options =>
            {
                options.Password = passwordOptions;
                options.ClaimsIdentity.SecurityStampClaimType = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddEntityFrameworkStores<CustomersDb>()
            .AddDefaultTokenProviders();

        return services;
    }
}