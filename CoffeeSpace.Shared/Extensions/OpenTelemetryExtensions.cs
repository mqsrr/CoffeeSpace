using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;

namespace CoffeeSpace.Shared.Extensions;

public static class OpenTelemetryExtensions
{
    public static IServiceCollection AddOpenTelemetryWithPrometheusExporter(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .WithMetrics(providerBuilder =>
            {
                providerBuilder.AddPrometheusExporter();
                providerBuilder.AddMeter("Microsoft.AspNetCore.Hosting", "Microsoft.AspNetCore.Server.Kestrel");

                providerBuilder.AddView("request-duration", new ExplicitBucketHistogramConfiguration
                {
                    Boundaries = [0, .005, .01, .025, .05, .075, .1, .25, .5, .7, .9]
                });
            });
        
        return services;
    }
}