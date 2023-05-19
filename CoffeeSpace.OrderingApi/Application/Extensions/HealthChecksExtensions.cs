namespace CoffeeSpace.OrderingApi.Application.Extensions;

public static class HealthChecksExtensions
{
    public static IHealthChecksBuilder AddServiceHealthChecks(this IServiceCollection services, WebApplicationBuilder builder)
    {
        return services.AddHealthChecks()
            .AddMySql(builder.Configuration["OrderingDb:ConnectionString"]!, name:"OrderingDb", tags: new [] {"Database"})
            .AddMySql(builder.Configuration["OrderStateSagaDb:ConnectionString"]!, name:"OrderStateSagaDb", tags: new [] {"Database"})
            .AddRedis(builder.Configuration["Redis:ConnectionString"]!, name:"Redis", tags: new [] {"Service"});

    }
}