using CoffeeSpace.Shared.Extensions;
using CoffeeSpace.Shared.Settings;
using CoffeeSpace.ShipmentService.Consumers;
using MassTransit;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddOpenTelemetryWithInstrumentation();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Configuration.AddAzureKeyVault();

builder.Services.AddOptionsWithValidateOnStart<AwsMessagingSettings>()
    .Bind(builder.Configuration.GetRequiredSection(AwsMessagingSettings.SectionName));

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();
    busConfigurator.AddConsumer<RequestOrderShipmentConsumer>();
    
    busConfigurator.UsingAmazonSqs((context, configurator) =>
    {
        var awsSettings = context.GetRequiredService<IOptions<AwsMessagingSettings>>().Value;
        configurator.Host(awsSettings.Region, hostConfigurator =>
        {
            hostConfigurator.AccessKey(awsSettings.AccessKey);
            hostConfigurator.SecretKey(awsSettings.SecretKey);
        });
        configurator.ConfigureEndpoints(context);

        configurator.UseNewtonsoftJsonSerializer();
        configurator.UseNewtonsoftJsonDeserializer();
    });
});

builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.MapPrometheusScrapingEndpoint();

app.UseHealthChecks("/_health");
app.Run();