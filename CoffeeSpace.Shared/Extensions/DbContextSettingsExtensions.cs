using CoffeeSpace.Shared.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;

namespace CoffeeSpace.Shared.Extensions;

public static class DbContextSettingsExtensions
{
    public static IServiceCollection AddNpgsqlDbContextOptions<TDbContext>(this IServiceCollection services, string connectionString)
        where TDbContext : DbContext
    {
        services.AddTransient<IOptions<PostgresDbContextSettings<TDbContext>>>(_ =>
        {
            var builder = new NpgsqlConnectionStringBuilder(connectionString);
            return Options.Create(new PostgresDbContextSettings<TDbContext>
            {
                ConnectionString = connectionString,
                Database = builder.Database!,
                Host = builder.Host!,
                Port = builder.Port
            });
        });

        return services;
    }
}