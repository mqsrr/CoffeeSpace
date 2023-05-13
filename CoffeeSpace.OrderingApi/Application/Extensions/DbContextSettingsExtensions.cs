using CoffeeSpace.Application.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace CoffeeSpace.OrderingApi.Application.Extensions;

internal static class DbContextSettingsExtensions
{
    public static IServiceCollection AddDbContextOptions<TDbContext>(this IServiceCollection services, string connectionString)
        where TDbContext : DbContext
    {
        services.AddTransient<IOptions<MySqlDbContextSettings<TDbContext>>>(_ =>
        {
            var mySqlbuilder = new MySqlConnectionStringBuilder(connectionString);
            return Options.Create(new MySqlDbContextSettings<TDbContext>
            {
                ConnectionString = mySqlbuilder.ConnectionString,
                Database = mySqlbuilder.Database,
                Host = mySqlbuilder.Server,
                Port = mySqlbuilder.Port,
                ServerVersion = ServerVersion.AutoDetect(mySqlbuilder.ConnectionString)
            });
        });

        return services;
    }
}