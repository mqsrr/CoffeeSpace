using Asp.Versioning;
using CoffeeSpace.Messages.Buyers;
using CoffeeSpace.Messages.Ordering.Commands;
using CoffeeSpace.Messages.Ordering.Responses;
using CoffeeSpace.Messages.Payment;
using CoffeeSpace.Messages.Products.Commands;
using CoffeeSpace.Messages.Products.Responses;
using CoffeeSpace.Messages.Shipment.Commands;
using CoffeeSpace.Messages.Shipment.Responses;
using CoffeeSpace.OrderingApi.Application.Extensions;
using CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Consumers;
using CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Sagas;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using CoffeeSpace.OrderingApi.Application.Services.Abstractions;
using CoffeeSpace.OrderingApi.Application.Settings;
using CoffeeSpace.OrderingApi.Application.SignalRHubs;
using CoffeeSpace.OrderingApi.Application.Validators;
using CoffeeSpace.OrderingApi.Persistence;
using CoffeeSpace.OrderingApi.Persistence.Abstractions;
using CoffeeSpace.Shared.Extensions;
using CoffeeSpace.Shared.Services.Abstractions;
using CoffeeSpace.Shared.Settings;
using Confluent.Kafka;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddOpenTelemetryWithInstrumentation();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Configuration.AddAzureKeyVault();
builder.Configuration.AddJwtBearer(builder);

builder.Services.AddSignalR().AddAzureSignalR();

builder.Services.AddControllers();
builder.Services.AddMediator();

builder.Services.AddApiVersioning(new HeaderApiVersionReader());

builder.Services.AddApplicationDb<IOrderingDbContext, OrderingDbContext>(builder.Configuration["OrderingDb:ConnectionString"]!);
builder.Services.AddApplicationDb<OrderStateSagaDbContext>(builder.Configuration["OrderStateSagaDb:ConnectionString"]!);

builder.Services.AddStackExchangeRedisCache(options => options.Configuration = builder.Configuration["Redis:ConnectionString"]);
builder.Services.AddApplicationService<ICacheService>();

builder.Services.AddApplicationService<IOrderService>();
builder.Services.AddApplicationService<IBuyerService>();

builder.Services.AddApplicationService<IOrderRepository>();
builder.Services.AddApplicationService<IBuyerRepository>();

builder.Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<IValidatorMarker>(ServiceLifetime.Singleton, includeInternalTypes: true);

builder.Services.AddOptionsWithValidateOnStart<JwtSettings>()
    .Bind(builder.Configuration.GetRequiredSection(JwtSettings.SectionName));

builder.Services.AddCors(options => options.AddDefaultPolicy(policyBuilder =>
    policyBuilder.AllowCredentials()
        .WithOrigins(CorsSettings.AllowedOrigins)
        .WithMethods(CorsSettings.AllowedMethods)
        .WithHeaders(CorsSettings.AllowedHeaders)));

builder.Services.AddMassTransit(x =>
{
    x.UsingInMemory();
    x.AddRider(configurator =>
    {
        configurator.SetKebabCaseEndpointNameFormatter();
        configurator.AddConsumersFromNamespaceContaining<RegisterNewBuyerConsumer>();
        
        configurator.AddProducer<SubmitOrder>("submit-order");
        configurator.AddProducer<ValidateOrderStock>("validate-order-stock");
        configurator.AddProducer<UpdateBuyer>("update-buyer");
        configurator.AddProducer<DeleteBuyer>("delete-buyer");
        configurator.AddProducer<RequestOrderPayment>("request-order-payment");
        configurator.AddProducer<RequestOrderShipment>("request-order-shipment");
        
        configurator.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>()
            .EntityFrameworkRepository(repositoryConfigurator =>
            {
                repositoryConfigurator.ConcurrencyMode = ConcurrencyMode.Optimistic;
                repositoryConfigurator.UsePostgres();
                repositoryConfigurator.ExistingDbContext<OrderStateSagaDbContext>();
            });
        
        configurator.UsingKafka((context, kafkaConfigurator) =>
        {
            kafkaConfigurator.Host(builder.Configuration["Kafka:Host"]);
            kafkaConfigurator.Acks = Acks.All;

            kafkaConfigurator
                .AddTopicEndpoint<RegisterNewBuyer, RegisterNewBuyerConsumer>(context, "register-customer", "ordering")
                .AddTopicEndpoint<PaymentPageInitialized, PaymentPageInitializedConsumer>(context, "order-payment-initialized", "ordering");

            kafkaConfigurator
                .AddSagaTopicEndpoint<SubmitOrder, OrderStateInstance>(context, "submit-order", "ordering")
                .AddSagaTopicEndpoint<OrderStockConfirmed, OrderStateInstance>(context, "order-stock-confirmed", "ordering")
                .AddSagaTopicEndpoint<Fault<ValidateOrderStock>, OrderStateInstance>(context, "order-stock-confirmation-failed", "ordering")
                .AddSagaTopicEndpoint<OrderPaid, OrderStateInstance>(context, "order-paid", "ordering")
                .AddSagaTopicEndpoint<OrderShipped, OrderStateInstance>(context, "order-shipped", "ordering");
        });
    });
});

builder.Services.AddServiceHealthChecks(builder);

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseHealthChecks("/_health");

app.MapPrometheusScrapingEndpoint();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<OrderingHub>("ordering-hub");

app.Run();