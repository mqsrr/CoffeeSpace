namespace CoffeeSpace.PaymentService.Application.Extensions;

public static class HealthChecksExtensions
{
    public static IHealthChecksBuilder AddServiceHealthChecks(this IServiceCollection services, WebApplicationBuilder builder)
    {
        return services.AddHealthChecks()
            .AddRedis(builder.Configuration["Redis:ConnectionString"]!, name:"Redis", tags: ["Service"]);
    }
}