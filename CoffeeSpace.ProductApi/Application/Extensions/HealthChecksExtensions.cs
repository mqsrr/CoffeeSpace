namespace CoffeeSpace.ProductApi.Application.Extensions;

public static class HealthChecksExtensions
{
    public static IHealthChecksBuilder AddServiceHealthChecks(this IServiceCollection services, WebApplicationBuilder builder)
    {
        return services.AddHealthChecks()
            .AddNpgSql(builder.Configuration["ProductsDb:ConnectionString"]!, name: "ProductsDb", tags: new[] {"Database"})
            .AddRedis(builder.Configuration["Redis:ConnectionString"]!, name: "Redis", tags: new[] {"Service"});
    }
}