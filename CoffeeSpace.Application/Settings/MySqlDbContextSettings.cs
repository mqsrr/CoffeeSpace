using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.Application.Settings;

public sealed class MySqlDbContextSettings<TDbContext>
    where TDbContext : DbContext
{
    public required string ConnectionString { get; init; }

    public required string Host { get; init; }
    
    public required uint Port { get; init; }
    
    public required string Database { get; init; }

    public required ServerVersion ServerVersion { get; init; }
}