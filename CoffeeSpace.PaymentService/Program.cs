using CoffeeSpace.Messages;
using CoffeeSpace.Messages.Payment.Commands;
using CoffeeSpace.PaymentService.Application.Extensions;
using CoffeeSpace.PaymentService.Application.Messages.Consumers;
using CoffeeSpace.PaymentService.Application.Services.Abstractions;
using CoffeeSpace.PaymentService.Application.Settings;
using CoffeeSpace.PaymentService.Models;
using CoffeeSpace.Shared.Extensions;
using CoffeeSpace.Shared.Settings;
using MassTransit;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddOpenTelemetryWithInstrumentation();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Configuration.AddAzureKeyVault();
builder.Services.AddApplicationService<IPaymentService>();

builder.Services.AddOptionsWithValidateOnStart<PaypalAuthenticationSettings>()
    .Bind(builder.Configuration.GetRequiredSection(PaypalAuthenticationSettings.SectionName));

builder.Services.AddOptionsWithValidateOnStart<AwsMessagingSettings>()
    .Bind(builder.Configuration.GetRequiredSection(AwsMessagingSettings.SectionName));

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();
    busConfigurator.AddConsumer<OrderPaymentValidationConsumer>();
    
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
        
        EndpointConvention.Map<PaymentPageInitialized>(new Uri(EndpointAddresses.Payment.PaymentPageInitialized));
    });
});
builder.Services.AddServiceHealthChecks(builder);

var app = builder.Build();

app.MapPost("/", async (OrderApprovedWebhookEvent order, IPaymentService paymentService, CancellationToken cancellationToken) =>
{
    await paymentService.CapturePaypalPaymentAsync(order, cancellationToken);
    return TypedResults.Ok();
});

app.UseSerilogRequestLogging();
app.MapPrometheusScrapingEndpoint();

app.UseHealthChecks("/_health");

app.Run();