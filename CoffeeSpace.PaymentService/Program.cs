using CoffeeSpace.PaymentService.Application.Extensions;
using CoffeeSpace.PaymentService.Application.Messages.Consumers;
using CoffeeSpace.PaymentService.Application.Messages.PipelineBehaviours;
using CoffeeSpace.PaymentService.Application.Repositories.Abstractions;
using CoffeeSpace.PaymentService.Application.Services.Abstractions;
using CoffeeSpace.PaymentService.Application.Settings;
using CoffeeSpace.PaymentService.Persistence;
using CoffeeSpace.PaymentService.Persistence.Abstractions;
using CoffeeSpace.Shared.Extensions;
using CoffeeSpace.Shared.Settings;
using FastEndpoints;
using MassTransit;
using Mediator;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddAzureKeyVault();
builder.Configuration.AddJwtBearer(builder);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
        .AddDatadogLogging("Payment Service"));

builder.Services.AddApplicationDb<IPaymentDbContext, PaymentDbContext>(builder.Configuration["PaymentDb:ConnectionString"]!);

builder.Services.AddApplicationService<IPaymentRepository>();
builder.Services.AddApplicationService<IPaymentService>();

builder.Services.AddMediator();
builder.Services.AddFastEndpoints();

builder.Services.AddApplicationService(typeof(IPipelineBehavior<,>), typeof(IPipelineAssemblyMarker));

builder.Services.AddOptionsWithValidateOnStart<AwsMessagingSettings>()
    .Bind(builder.Configuration.GetRequiredSection(AwsMessagingSettings.SectionName));

builder.Services.AddOptionsWithValidateOnStart<PaypalAuthenticationSettings>()
    .Bind(builder.Configuration.GetRequiredSection(PaypalAuthenticationSettings.SectionName));

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddConsumer<OrderPaymentValidationConsumer>();
    
    x.UsingAmazonSqs((context, config) =>
    {
        var awsSettings = context.GetRequiredService<IOptions<AwsMessagingSettings>>().Value;
        config.Host(awsSettings.Region, hostConfig =>
        {
            hostConfig.AccessKey(awsSettings.AccessKey);
            hostConfig.SecretKey(awsSettings.SecretKey);
        });
        config.UseNewtonsoftJsonSerializer();
        config.UseNewtonsoftJsonDeserializer();

        config.ConfigureEndpoints(context);
    });
});

builder.Services.AddServiceHealthChecks(builder);

var app = builder.Build();
app.UseHealthChecks("/_health");

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();
app.Run();