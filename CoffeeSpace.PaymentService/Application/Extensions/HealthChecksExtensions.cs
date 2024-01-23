namespace CoffeeSpace.PaymentService.Application.Extensions;

public static class HealthChecksExtensions
{
    public static IHealthChecksBuilder AddServiceHealthChecks(this IServiceCollection services, WebApplicationBuilder builder)
    {
        return services.AddHealthChecks()
            .AddNpgSql(builder.Configuration["PaymentDb:ConnectionString"]!, name:"PaymentDb", tags: new[] {"Database"})
            .AddRedis(builder.Configuration["Redis:ConnectionString"]!, name:"Redis", tags: new []{"Service"});
    }
}