using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace CoffeeSpace.Shared.Extensions;

public static class OpenTelemetryExtensions
{
    public static WebApplicationBuilder AddOpenTelemetryWithInstrumentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenTelemetry()
            .WithMetrics(providerBuilder =>
            {
                providerBuilder.AddRuntimeInstrumentation()
                    .AddMeter("Microsoft.AspNetCore.Hosting", "Microsoft.AspNetCore.Server.Kestrel","System.Net.Http");
            })
            .WithTracing(providerBuilder =>
            {
                providerBuilder.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();
            });

        builder.Services.Configure<OpenTelemetryLoggerOptions>(logging => logging.AddOtlpExporter());
        builder.Services.ConfigureOpenTelemetryMeterProvider(logging => logging.AddOtlpExporter());
        builder.Services.ConfigureOpenTelemetryTracerProvider(logging => logging.AddOtlpExporter());
        
        builder.Services.AddOpenTelemetry().WithMetrics(providerBuilder => providerBuilder.AddPrometheusExporter());
        
        builder.Logging.AddOpenTelemetry(options =>
        {
            options.IncludeScopes = true;
            options.IncludeFormattedMessage = true;
        });
        
        return builder;
    }
}