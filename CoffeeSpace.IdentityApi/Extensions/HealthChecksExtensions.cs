namespace CoffeeSpace.IdentityApi.Extensions;

public static class HealthChecksExtensions
{
    public static IHealthChecksBuilder AddServiceHealthChecks(this IServiceCollection services, WebApplicationBuilder builder)
    {
        return services.AddHealthChecks()
            .AddNpgSql(builder.Configuration["IdentityDb:ConnectionString"]!, name:"IdentityDb", tags: new[] {"Database"})
            .AddRedis(builder.Configuration["Redis:ConnectionString"]!, name:"Redis", tags: new []{"Service"});
    }
}