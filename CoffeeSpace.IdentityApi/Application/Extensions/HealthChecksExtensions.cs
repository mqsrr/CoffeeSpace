namespace CoffeeSpace.IdentityApi.Application.Extensions;

public static class HealthChecksExtensions
{
    public static IHealthChecksBuilder AddServiceHealthChecks(this IServiceCollection services, WebApplicationBuilder builder)
    {
        return services.AddHealthChecks()
            .AddNpgSql(builder.Configuration["IdentityDb:ConnectionString"]!, name:"IdentityDb", tags: ["Database"]);
    }
}