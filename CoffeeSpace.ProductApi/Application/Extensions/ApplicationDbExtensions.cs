using CoffeeSpace.Application.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.ProductApi.Application.Extensions;

internal static class ApplicationDbExtensions
{
    public static IServiceCollection AddApplicationDb<TDbContext>(this IServiceCollection services, string connectionString)
        where TDbContext : DbContext
    {
        services.AddMySql<TDbContext>(connectionString, ServerVersion.AutoDetect(connectionString));
        return services;
    }
    
    public static IServiceCollection AddApplicationDb<TDbInterface, TDbContext>(this IServiceCollection services, string connectionString)
        where TDbContext : DbContext, TDbInterface where TDbInterface : class
    {
        services.AddMySql<TDbContext>(connectionString, ServerVersion.AutoDetect(connectionString));
        services.AddApplicationService<TDbInterface>(services.Single(x => 
            x.ImplementationType == typeof(TDbContext)).Lifetime);
        
        return services;
    }
}