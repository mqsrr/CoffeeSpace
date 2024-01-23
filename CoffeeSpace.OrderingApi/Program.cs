using Asp.Versioning;
using CoffeeSpace.OrderingApi.Application.Extensions;
using CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Consumers;
using CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Sagas;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using CoffeeSpace.OrderingApi.Application.Services.Abstractions;
using CoffeeSpace.OrderingApi.Application.Services.Decorators;
using CoffeeSpace.OrderingApi.Application.Settings;
using CoffeeSpace.OrderingApi.Application.SignalRHubs;
using CoffeeSpace.OrderingApi.Application.Validators;
using CoffeeSpace.OrderingApi.Persistence;
using CoffeeSpace.OrderingApi.Persistence.Abstractions;
using CoffeeSpace.Shared.Extensions;
using CoffeeSpace.Shared.Services.Abstractions;
using CoffeeSpace.Shared.Settings;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.Extensions.Options;
using Quartz;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddAzureKeyVault();
builder.Configuration.AddJwtBearer(builder);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddSignalR().AddAzureSignalR();

builder.Services.AddControllers();
builder.Services.AddMediator();

builder.Services.AddApiVersioning(new MediaTypeApiVersionReader("api-version"));

builder.Services.AddApplicationDb<IOrderingDbContext, OrderingDbContext>(builder.Configuration["OrderingDb:ConnectionString"]!);
builder.Services.AddApplicationDb<OrderStateSagaDbContext>(builder.Configuration["OrderStateSagaDb:ConnectionString"]!);

builder.Services.AddApplicationService<ICacheService>();

builder.Services.Decorate<IOrderService, CachedOrderService>();
builder.Services.Decorate<IBuyerService, CachedBuyerService>();

builder.Services.AddApplicationService<IOrderService>();
builder.Services.AddApplicationService<IOrderRepository>();

builder.Services.AddApplicationService<IBuyerService>();
builder.Services.AddApplicationService<IBuyerRepository>();

builder.Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<IValidatorMarker>(ServiceLifetime.Singleton, includeInternalTypes: true);

builder.Services.AddOptionsWithValidateOnStart<AwsMessagingSettings>()
    .Bind(builder.Configuration.GetRequiredSection(AwsMessagingSettings.SectionName));

builder.Services.AddOptionsWithValidateOnStart<JwtSettings>()
    .Bind(builder.Configuration.GetRequiredSection(JwtSettings.SectionName));

builder.Services.AddCors(options => options.AddDefaultPolicy(policyBuilder =>
    policyBuilder.AllowCredentials()
        .WithOrigins(CorsSettings.AllowedOrigins)
        .WithMethods(CorsSettings.AllowedMethods)
        .WithHeaders(CorsSettings.AllowedHeaders)));

builder.Services.AddQuartz(x =>
{
    x.UseInMemoryStore();

    x.InterruptJobsOnShutdownWithWait = true;
    x.UseTimeZoneConverter();
});

builder.Services.AddMassTransit(configurator =>
{
    configurator.SetKebabCaseEndpointNameFormatter();
    configurator.AddConsumersFromNamespaceContaining<RegisterNewBuyerConsumer>();

    configurator.AddQuartzConsumers();
    configurator.AddPublishMessageScheduler();
    
    configurator.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>()
        .EntityFrameworkRepository(repositoryConfigurator =>
        {
            repositoryConfigurator.ConcurrencyMode = ConcurrencyMode.Optimistic;

            repositoryConfigurator.UsePostgres();
            repositoryConfigurator.ExistingDbContext<OrderStateSagaDbContext>();
        });

    configurator.UsingAmazonSqs((context, config) =>
    {
        var awsMessagingSettings = context.GetRequiredService<IOptions<AwsMessagingSettings>>().Value;
        config.Host(awsMessagingSettings.Region, hostConfig =>
        {
            hostConfig.AccessKey(awsMessagingSettings.AccessKey);
            hostConfig.SecretKey(awsMessagingSettings.SecretKey);
        });

        config.UsePublishMessageScheduler();

        config.UseNewtonsoftJsonSerializer();
        config.UseNewtonsoftJsonDeserializer();

        config.ConfigureEndpoints(context);
    });
});

builder.Services.AddServiceHealthChecks(builder);
builder.Services.AddOpenTelemetryWithPrometheusExporter();

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