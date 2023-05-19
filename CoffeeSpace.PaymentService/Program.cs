using CoffeeSpace.Core.Extensions;
using CoffeeSpace.Core.Settings;
using CoffeeSpace.PaymentService.Consumers;
using CoffeeSpace.PaymentService.Extensions;
using CoffeeSpace.PaymentService.Persistence;
using CoffeeSpace.PaymentService.Repositories;
using CoffeeSpace.PaymentService.Repositories.Abstractions;
using MassTransit;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) => 
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Configuration.AddAzureKeyVault();

builder.Services.AddApplicationDb<IPaymentDbContext, PaymentDbContext>(builder.Configuration["PaymentDb:ConnectionString"]!);

builder.Services.AddApplicationService<IPaymentHistoryRepository>();

builder.Services.AddOptions<AwsMessagingSettings>()
    .Bind(builder.Configuration.GetRequiredSection("AWS"))
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

app.Run();