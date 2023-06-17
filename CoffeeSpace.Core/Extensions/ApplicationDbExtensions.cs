using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeSpace.Core.Extensions;

public static class ApplicationDbExtensions
{
    public static IServiceCollection AddApplicationDb<TDbContext>(this IServiceCollection services, string connectionString)
        where TDbContext : DbContext
    {
        services.AddNpgsql<TDbContext>(connectionString);
        services.AddNpgsqlDbContextOptions<TDbContext>(connectionString);
        
        return services;
    }
    
    public static IServiceCollection AddApplicationDb<TDbInterface, TDbContext>(this IServiceCollection services, string connectionString)
        where TDbContext : DbContext, TDbInterface 
        where TDbInterface : class
    {
        services.AddNpgsql<TDbContext>(connectionString);
        services.AddNpgsqlDbContextOptions<TDbContext>(connectionString);

        services.AddApplicationService<TDbInterface>(services.Single(x =>
            x.ImplementationType == typeof(TDbContext)).Lifetime);
        return services;
    }
}