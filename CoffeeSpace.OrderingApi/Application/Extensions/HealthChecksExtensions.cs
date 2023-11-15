namespace CoffeeSpace.OrderingApi.Application.Extensions;

public static class HealthChecksExtensions
{
    public static IHealthChecksBuilder AddServiceHealthChecks(this IServiceCollection services, WebApplicationBuilder builder)
    {
        return services.AddHealthChecks()
            .AddNpgSql(builder.Configuration["OrderingDb:ConnectionString"]!, name:"OrderingDb", tags: new [] {"Database"})
            .AddNpgSql(builder.Configuration["OrderStateSagaDb:ConnectionString"]!, name:"OrderStateSagaDb", tags: new [] {"Database"})
            .AddRedis(builder.Configuration["Redis:ConnectionString"]!, name:"Redis", tags: new [] {"Service"});
    }
}