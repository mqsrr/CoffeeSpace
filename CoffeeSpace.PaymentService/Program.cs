using CoffeeSpace.Core.Extensions;
using CoffeeSpace.Core.Services.Abstractions;
using CoffeeSpace.Core.Settings;
using CoffeeSpace.PaymentService.Consumers;
using CoffeeSpace.PaymentService.Extensions;
using CoffeeSpace.PaymentService.Messages.PipelineBehaviours;
using CoffeeSpace.PaymentService.Persistence;
using CoffeeSpace.PaymentService.Persistence.Abstractions;
using CoffeeSpace.PaymentService.Repositories.Abstractions;
using CoffeeSpace.PaymentService.Services.Abstractions;
using CoffeeSpace.PaymentService.Settings;
using FastEndpoints;
using MassTransit;
using Mediator;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Configuration.AddAzureKeyVault();
builder.Configuration.AddJwtBearer(builder);

builder.Services.AddStackExchangeRedisCache(options => options.Configuration = builder.Configuration["Redis:ConnectionString"]);
builder.Services.AddApplicationDb<IPaymentDbContext, PaymentDbContext>(builder.Configuration["PaymentDb:ConnectionString"]!);

builder.Services.AddApplicationService<IPaymentRepository>();
builder.Services.AddApplicationService<IPaymentService>();

builder.Services.AddMediator();
builder.Services.AddFastEndpoints();

builder.Services.AddApplicationService(typeof(ICacheService<>));
builder.Services.AddApplicationService(typeof(IPipelineBehavior<,>), typeof(IPipelineAssemblyMarker));

builder.Services.AddOptions<AwsMessagingSettings>()
    .Bind(builder.Configuration.GetRequiredSection(AwsMessagingSettings.SectionName))
    .ValidateOnStart();

builder.Services.AddOptions<PaypalAuthenticationSettings>()
    .Bind(builder.Configuration.GetRequiredSection(PaypalAuthenticationSettings.SectionName))
    .ValidateOnStart();

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