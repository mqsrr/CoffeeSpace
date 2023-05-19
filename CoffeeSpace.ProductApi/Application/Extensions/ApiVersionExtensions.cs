using Asp.Versioning;

namespace CoffeeSpace.ProductApi.Application.Extensions;

public static class ApiVersionExtensions
{
    public static IApiVersioningBuilder AddApiVersioning(this IServiceCollection services, IApiVersionReader reader, bool assumeDefaultVersion = true)
    {
        return services.AddApiVersioning(x =>
            {
                x.ApiVersionReader = reader;
                x.DefaultApiVersion = new ApiVersion(1.0);
                x.ReportApiVersions = true;
                x.AssumeDefaultVersionWhenUnspecified = assumeDefaultVersion;
            })
            .AddMvc();
    }
}