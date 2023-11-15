using Serilog;
using Serilog.Sinks.Datadog.Logs;

namespace CoffeeSpace.ApiGateway.Extensions;

public static class SerilogExtensions
{
    public static LoggerConfiguration AddDatadogLogging(this LoggerConfiguration loggerConfiguration, string serviceName)
    {
        loggerConfiguration.WriteTo.DatadogLogs(Environment.GetEnvironmentVariable("DD_API_KEY"),
            source: serviceName,
            service: serviceName,
            host: "CoffeeSpace",
            configuration: new DatadogConfiguration(" https://http-intake.logs.us5.datadoghq.com", 443));

        return loggerConfiguration;
    }
}