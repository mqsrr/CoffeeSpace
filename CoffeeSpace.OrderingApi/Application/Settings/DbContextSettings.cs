using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.OrderingApi.Application.Settings;

internal sealed class DbContextSettings<TDbContext>
    where TDbContext : DbContext
{
    public required string ConnectionString { get; init; }

    public required string Host { get; init; }
    
    public required uint Port { get; init; }
    
    public required string Database { get; init; }

    public required ServerVersion ServerVersion { get; init; }
}