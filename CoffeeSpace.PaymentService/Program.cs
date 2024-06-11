using CoffeeSpace.Messages.Payment;
using CoffeeSpace.PaymentService.Application.Extensions;
using CoffeeSpace.PaymentService.Application.Messages.Consumers;
using CoffeeSpace.PaymentService.Application.Messages.PipelineBehaviours;
using CoffeeSpace.PaymentService.Application.Repositories.Abstractions;
using CoffeeSpace.PaymentService.Application.Services.Abstractions;
using CoffeeSpace.PaymentService.Application.Settings;
using CoffeeSpace.PaymentService.Persistence;
using CoffeeSpace.PaymentService.Persistence.Abstractions;
using CoffeeSpace.Shared.Extensions;
using CoffeeSpace.Shared.Services.Abstractions;
using CoffeeSpace.Shared.Settings;
using FastEndpoints;
using MassTransit;
using MassTransit.Configuration;
using Mediator;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddOpenTelemetryWithInstrumentation();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Configuration.AddAzureKeyVault();
builder.Configuration.AddJwtBearer(builder);

builder.Services.AddApplicationDb<IPaymentDbContext, PaymentDbContext>("Server=localhost;Port=5436;Database=testDb;User Id=test;Password=Test1234!;");

builder.Services.AddApplicationService<IPaymentRepository>();
builder.Services.AddApplicationService<IPaymentService>();

builder.Services.AddMediator();
builder.Services.AddFastEndpoints();

builder.Services.AddApplicationService(typeof(IPipelineBehavior<,>), typeof(IPipelineAssemblyMarker));

builder.Services.AddStackExchangeRedisCache(options => options.Configuration = "localhost:6379");
builder.Services.AddApplicationService<ICacheService>();

builder.Services.AddOptionsWithValidateOnStart<PaypalAuthenticationSettings>()
    .Bind(builder.Configuration.GetRequiredSection(PaypalAuthenticationSettings.SectionName));

builder.Services.AddOptionsWithValidateOnStart<AwsMessagingSettings>()
    .Bind(builder.Configuration.GetRequiredSection(AwsMessagingSettings.SectionName));

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<OrderPaymentValidationConsumer>();
    busConfigurator.AddInMemoryInboxOutbox();
    
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
        
        EndpointConvention.Map<PaymentPageInitialized>(new Uri("queue:payment-page-initialized"));
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