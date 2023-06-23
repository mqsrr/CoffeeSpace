using System.Data;
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

builder.Services.AddControllers();
builder.Services.AddMediator();

builder.Services.AddApiVersioning(new MediaTypeApiVersionReader("api-version"));

builder.Services.AddStackExchangeRedisCache(x =>
    x.Configuration = builder.Configuration["Redis:ConnectionString"]);

builder.Services.AddApplicationDb<IOrderingDbContext, OrderingDbContext>(builder.Configuration["OrderingDb:ConnectionString"]!);
builder.Services.AddApplicationDb<OrderStateSagaDbContext>(builder.Configuration["OrderStateSagaDb:ConnectionString"]!);

builder.Services.AddApplicationService(typeof(ICacheService<>));

builder.Services.AddApplicationService<IOrderService>();
builder.Services.AddApplicationService<IOrderRepository>();

builder.Services.AddApplicationService<IBuyerService>();
builder.Services.AddApplicationService<IBuyerRepository>();

builder.Services.AddApplicationService(typeof(IPipelineBehavior<,>), typeof(IPipelineAssemblyMarker));

builder.Services.AddOptions<AwsMessagingSettings>()
    .Bind(builder.Configuration.GetSection("AWS"))
    .ValidateOnStart();

builder.Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<IValidatorMarker>(ServiceLifetime.Singleton, includeInternalTypes: true);

builder.Services.AddQuartz(x => x.UseMicrosoftDependencyInjectionJobFactory());

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddConsumer<RegisterNewBuyerConsumer>();
    x.AddConsumer<UpdateOrderStatusConsumer>();

    x.AddQuartzConsumers();
    x.AddPublishMessageScheduler();

    x.AddEntityFrameworkOutbox<OrderStateSagaDbContext>(configurator =>
    {
        configurator.IsolationLevel = IsolationLevel.ReadCommitted;        
        configurator.UsePostgres();
    });
     
    x.AddSagaStateMachine<OrderStateMachine, OrderStateInstance, OrderStateDefinition>()
        .EntityFrameworkRepository(configurator =>
        {
            configurator.ConcurrencyMode = ConcurrencyMode.Optimistic;
            
            configurator.UsePostgres();
            configurator.ExistingDbContext<OrderStateSagaDbContext>();
        });

    x.UsingAmazonSqs((context, config) =>
    {
        var awsSettings = context.GetRequiredService<IOptions<AwsMessagingSettings>>().Value;
        config.Host(awsSettings.Region, hostConfig =>
        {
            hostConfig.AccessKey(awsSettings.AccessKey);
            hostConfig.SecretKey(awsSettings.SecretKey);
        });
        config.UsePublishMessageScheduler();

        config.UseNewtonsoftJsonSerializer();
        config.UseNewtonsoftJsonSerializer();
        
        config.ConfigureEndpoints(context);
    });
});

builder.Services.AddServiceHealthChecks(builder);

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseHealthChecks("/_health");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();