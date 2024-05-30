using Asp.Versioning;
using CoffeeSpace.Messages.Products.Commands;
using CoffeeSpace.Messages.Products.Responses;
using CoffeeSpace.ProductApi.Application.Extensions;
using CoffeeSpace.ProductApi.Application.Messages.Consumers;
using CoffeeSpace.ProductApi.Application.Repositories;
using CoffeeSpace.ProductApi.Application.Repositories.Abstractions;
using CoffeeSpace.ProductApi.Application.Validators;
using CoffeeSpace.ProductApi.Persistence;
using CoffeeSpace.ProductApi.Persistence.Abstractions;
using CoffeeSpace.Shared.Extensions;
using CoffeeSpace.Shared.Services.Abstractions;
using CoffeeSpace.Shared.Settings;
using Confluent.Kafka;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddOpenTelemetryWithInstrumentation();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Configuration.AddAzureKeyVault();
builder.Configuration.AddJwtBearer(builder);

builder.Services.AddControllers();
builder.Services.AddApiVersioning(new HeaderApiVersionReader());

builder.Services.AddApplicationDb<IProductDbContext, ProductDbContext>(builder.Configuration["ProductsDb:ConnectionString"]!);
builder.Services.AddApplicationService<IProductRepository>();

builder.Services.AddStackExchangeRedisCache(options => options.Configuration = builder.Configuration["Redis:ConnectionString"]);
builder.Services.AddApplicationService<ICacheService>();

builder.Services.Decorate<IProductRepository, CachedProductRepository>();

builder.Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>(ServiceLifetime.Singleton, includeInternalTypes: true);

builder.Services.AddOptionsWithValidateOnStart<JwtSettings>()
    .Bind(builder.Configuration.GetRequiredSection(JwtSettings.SectionName));

builder.Services.AddOptionsWithValidateOnStart<KafkaSettings>()
    .Bind(builder.Configuration.GetRequiredSection(KafkaSettings.SectionName))
    .Configure(settings => settings.Hosts = JsonConvert.DeserializeObject<IReadOnlyList<string>>(builder.Configuration["Kafka:Hosts"]!)!);

builder.Services.AddMassTransit(x =>
{
    x.UsingInMemory();
    x.AddRider(configurator =>
    {
        configurator.SetKebabCaseEndpointNameFormatter();
        configurator.AddConsumer<OrderStockValidationConsumer>();

        configurator.AddProducer<OrderStockConfirmed>("order-stock-confirmed");
        configurator.AddProducer<Fault<ValidateOrderStock>>("order-stock-confirmation-failed");

        configurator.AddInMemoryInboxOutbox();
        configurator.UsingKafka((context, factoryConfigurator) =>
        {
            var kafkaSettings = context.GetRequiredService<IOptions<KafkaSettings>>().Value;
            factoryConfigurator.Acks = Acks.All;
            
            factoryConfigurator.Host(kafkaSettings.Hosts);
            factoryConfigurator.AddTopicEndpoint<ValidateOrderStock, OrderStockValidationConsumer>(context, "validate-order-stock", "products");
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

app.MapControllers();
app.Run();