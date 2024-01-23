using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Sinks.Datadog.Logs;

namespace CoffeeSpace.Shared.Extensions;

public static class SerilogExtensions
{
    public static LoggerConfiguration AddDatadogLogging(this LoggerConfiguration loggerConfiguration, string serviceName)
    {
        if (!Environment.GetEnvironmentVariable(Environments.Staging).IsNullOrEmpty())
        {
            return loggerConfiguration;
        }
        
        return loggerConfiguration.WriteTo.DatadogLogs(Environment.GetEnvironmentVariable("DD_API_KEY"),
            service: serviceName,
            host: "CoffeeSpace",
            configuration: new DatadogConfiguration(" https://http-intake.logs.us5.datadoghq.com", 443));
    }
}