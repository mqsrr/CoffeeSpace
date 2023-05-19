using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.PaymentService.Settings;

public sealed class PostgresDbContextSettings<TDbContext>
    where TDbContext : DbContext
{
    public required string ConnectionString { get; init; }
    
    public required string Database { get; init; }
    
    public required string Host { get; init; }
    
    public required int Port { get; init; }
}