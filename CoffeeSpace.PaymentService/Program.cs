using CoffeeSpace.Messages;
using CoffeeSpace.Messages.Payment.Commands;
using CoffeeSpace.PaymentService.Application.Extensions;
using CoffeeSpace.PaymentService.Application.Messages.Consumers;
using CoffeeSpace.PaymentService.Application.Repositories.Abstractions;
using CoffeeSpace.PaymentService.Application.Services.Abstractions;
using CoffeeSpace.PaymentService.Application.Settings;
using CoffeeSpace.PaymentService.Persistence;
using CoffeeSpace.PaymentService.Persistence.Abstractions;
using CoffeeSpace.Shared.Extensions;
using CoffeeSpace.Shared.Settings;
using FastEndpoints;
using MassTransit;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddOpenTelemetryWithInstrumentation();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Configuration.AddAzureKeyVault();
builder.Configuration.AddJwtBearer(builder);

builder.Services.AddApplicationDb<IPaymentDbContext, PaymentDbContext>(builder.Configuration["PaymentDb:ConnectionString"]!);

builder.Services.AddApplicationService<IPaymentRepository>();
builder.Services.AddApplicationService<IPaymentService>();

builder.Services.AddFastEndpoints();

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

app.UseSerilogRequestLogging();
app.MapPrometheusScrapingEndpoint();

app.UseHealthChecks("/_health");
app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();
app.Run();