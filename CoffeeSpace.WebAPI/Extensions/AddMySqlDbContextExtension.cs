using Microsoft.EntityFrameworkCore;

namespace CoffeeSpace.WebAPI.Extensions;

public static class AddMySqlDbContextExtension 
{
    public static WebApplicationBuilder AddMySqlDbContext<TContext>(this WebApplicationBuilder builder, string jsonPath)
        where TContext : DbContext
    {
        string connectionString = builder.Configuration[jsonPath]!;

        builder.Services.AddMySql<TContext>(connectionString, ServerVersion.AutoDetect(connectionString));
        
        return builder;
    }
}