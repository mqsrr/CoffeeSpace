using CoffeeSpace.Messages.Ordering.Commands;
using CoffeeSpace.Messages.Ordering.Responses;
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
using Confluent.Kafka;
using FastEndpoints;
using MassTransit;
using Mediator;
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

builder.Services.AddMediator();
builder.Services.AddFastEndpoints();

builder.Services.AddApplicationService(typeof(IPipelineBehavior<,>), typeof(IPipelineAssemblyMarker));

builder.Services.AddStackExchangeRedisCache(options => options.Configuration = builder.Configuration["Redis:ConnectionString"]);
builder.Services.AddApplicationService<ICacheService>();

builder.Services.AddOptionsWithValidateOnStart<PaypalAuthenticationSettings>()
    .Bind(builder.Configuration.GetRequiredSection(PaypalAuthenticationSettings.SectionName));

builder.Services.AddMassTransit(x =>
{
    x.UsingInMemory();
    x.AddRider(configurator =>
    {
        configurator.SetKebabCaseEndpointNameFormatter();

        configurator.AddProducer<PaymentPageInitialized>("order-payment-initialized");
        configurator.AddProducer<OrderPaid>("order-paid");
        configurator.AddProducer<Fault<RequestOrderPayment>>("order-payment-failed");
        
        configurator.AddConsumer<OrderPaymentValidationConsumer>();
        configurator.UsingKafka((context, kafkaConfigurator) =>
        {
            kafkaConfigurator.Host(builder.Configuration["Kafka:Host"]);
            
            kafkaConfigurator.Acks = Acks.All;
            kafkaConfigurator.AddTopicEndpoint<RequestOrderPayment, OrderPaymentValidationConsumer>(context, "request-order-payment", "payment");
        });
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