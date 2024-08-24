namespace CoffeeSpace.OrderingApi.Application.Extensions;

public static class HealthChecksExtensions
{
    public static IHealthChecksBuilder AddServiceHealthChecks(this IServiceCollection services, WebApplicationBuilder builder)
    {
        return services.AddHealthChecks()
            .AddNpgSql(builder.Configuration["OrderingDb:ConnectionString"]!, name:"OrderingDb", tags: ["Database"])
            .AddRedis(builder.Configuration["Redis:ConnectionString"]!, name:"Redis", tags: ["Service"]);
    }
}