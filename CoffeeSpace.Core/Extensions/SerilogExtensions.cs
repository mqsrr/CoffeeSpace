using System.Reflection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Datadog.Logs;

namespace CoffeeSpace.Core.Extensions;

public static class SerilogExtensions
{
    public static LoggerConfiguration AddDatadogLogging(this LoggerConfiguration loggerConfiguration)
    {
        string solutionName = AppDomain.CurrentDomain.FriendlyName;
        loggerConfiguration.WriteTo.DatadogLogs(Environment.GetEnvironmentVariable("DD_API_KEY"),
            service: solutionName,
            host: "CoffeeSpace",
            configuration: new DatadogConfiguration(" https://http-intake.logs.us5.datadoghq.com", 443));

        return loggerConfiguration;
    }
    
    public static LoggerConfiguration AddDatadogLogging(this LoggerConfiguration loggerConfiguration, string serviceName)
    {
        loggerConfiguration.WriteTo.DatadogLogs(Environment.GetEnvironmentVariable("DD_API_KEY"),
            service: serviceName,
            host: "CoffeeSpace",
            configuration: new DatadogConfiguration(" https://http-intake.logs.us5.datadoghq.com", 443));

        return loggerConfiguration;
    }
}