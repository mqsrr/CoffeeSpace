using Asp.Versioning;
using CoffeeSpace.Core.Extensions;
using CoffeeSpace.Core.Services.Abstractions;
using CoffeeSpace.Core.Settings;
using CoffeeSpace.OrderingApi.Application.Extensions;
using CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Consumers;
using CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Sagas;
using CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Sagas.Definitions;
using CoffeeSpace.OrderingApi.Application.Pipelines;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using CoffeeSpace.OrderingApi.Application.Services.Abstractions;
using CoffeeSpace.OrderingApi.Application.SignalRHubs;
using CoffeeSpace.OrderingApi.Application.Validators;
using CoffeeSpace.OrderingApi.Persistence;
using CoffeeSpace.OrderingApi.Persistence.Abstractions;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Mediator;
using Microsoft.Extensions.Options;
using Quartz;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Configuration.AddAzureKeyVault();
builder.Configuration.AddJwtBearer(builder);

builder.Services.AddSignalR().AddAzureSignalR();
builder.Services.AddControllers();
builder.Services.AddMediator();

builder.Services.AddApiVersioning(new MediaTypeApiVersionReader("api-version"));
builder.Services.AddStackExchangeRedisCache(x => x.Configuration = builder.Configuration["Redis:ConnectionString"]);

builder.Services.AddApplicationDb<IOrderingDbContext, OrderingDbContext>(builder.Configuration["OrderingDb:ConnectionString"]!);
builder.Services.AddApplicationDb<OrderStateSagaDbContext>(builder.Configuration["OrderStateSagaDb:ConnectionString"]!);

builder.Services.AddApplicationService(typeof(ICacheService<>));

builder.Services.AddApplicationService<IOrderService>();
builder.Services.AddApplicationService<IOrderRepository>();

builder.Services.AddApplicationService<IBuyerService>();
builder.Services.AddApplicationService<IBuyerRepository>();

builder.Services.AddApplicationService(typeof(IPipelineBehavior<,>), typeof(IPipelineAssemblyMarker));

builder.Services.AddOptions<AwsMessagingSettings>()
    .Bind(builder.Configuration.GetSection(AwsMessagingSettings.SectionName))
    .ValidateOnStart();

builder.Services.AddOptions<JwtSettings>()
    .Bind(builder.Configuration.GetSection(JwtSettings.SectionName))
    .ValidateOnStart();

builder.Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<IValidatorMarker>(ServiceLifetime.Singleton, includeInternalTypes: true);

builder.Services.AddCors(options => options.AddDefaultPolicy(policyBuilder =>
    policyBuilder.AllowCredentials()
        .WithOrigins("http://localhost:4200")
        .WithMethods("POST")
        .WithHeaders("x-requested-with", "x-signalr-user-agent")));

builder.Services.AddQuartz(x =>
{
    x.UseMicrosoftDependencyInjectionJobFactory();
    x.UseInMemoryStore();

    x.InterruptJobsOnShutdownWithWait = true;
    x.UseTimeZoneConverter();
});

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddConsumersFromNamespaceContaining<RegisterNewBuyerConsumer>();

    x.AddQuartzConsumers();
    x.AddPublishMessageScheduler();

    x.AddEntityFrameworkOutbox<OrderStateSagaDbContext>(configurator => configurator.UsePostgres());
    x.AddSagaStateMachine<OrderStateMachine, OrderStateInstance, OrderStateDefinition>()
        .EntityFrameworkRepository(configurator =>
        {
            configurator.ConcurrencyMode = ConcurrencyMode.Optimistic;

            configurator.UsePostgres();
            configurator.ExistingDbContext<OrderStateSagaDbContext>();
        });

    x.UsingAmazonSqs((context, config) =>
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

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseHealthChecks("/_health");

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<OrderingHub>("ordering-hub");

app.Run();