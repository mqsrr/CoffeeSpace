using Asp.Versioning;
using CoffeeSpace.Messages;
using CoffeeSpace.Messages.Buyers;
using CoffeeSpace.Messages.Ordering.Commands;
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
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.Extensions.Options;
using Quartz;
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

builder.Services.AddStackExchangeRedisCache(options => options.Configuration = builder.Configuration["Redis:ConnectionString"]!);
builder.Services.AddApplicationService<ICacheService>();

builder.Services.AddApplicationService<IOrderService>();
builder.Services.AddApplicationService<IBuyerService>();

builder.Services.AddApplicationService<IOrderRepository>();
builder.Services.AddApplicationService<IBuyerRepository>();

builder.Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<IValidatorMarker>(includeInternalTypes: true);

builder.Services.AddOptionsWithValidateOnStart<JwtSettings>()
    .Bind(builder.Configuration.GetRequiredSection(JwtSettings.SectionName));

builder.Services.AddOptionsWithValidateOnStart<AwsMessagingSettings>()
    .Bind(builder.Configuration.GetRequiredSection(AwsMessagingSettings.SectionName));

builder.Services.AddCors(options => options.AddDefaultPolicy(policyBuilder =>
    policyBuilder.AllowCredentials()
        .WithOrigins(CorsSettings.AllowedOrigins)
        .WithMethods(CorsSettings.AllowedMethods)
        .WithHeaders(CorsSettings.AllowedHeaders)));

builder.Services.AddQuartz(x =>
{
    x.UseInMemoryStore();
    x.InterruptJobsOnShutdownWithWait = true;
});

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();
    busConfigurator.AddConsumersFromNamespaceContaining<RegisterNewBuyerConsumer>();

    busConfigurator.AddQuartzConsumers();
    busConfigurator.AddPublishMessageScheduler();

    busConfigurator.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>()
        .InMemoryRepository();
    
    busConfigurator.UsingAmazonSqs((context, configurator) =>
    {
        var awsSettings = context.GetRequiredService<IOptions<AwsMessagingSettings>>().Value;
        configurator.Host(awsSettings.Region, hostConfigurator =>
        {
            hostConfigurator.AccessKey(awsSettings.AccessKey);
            hostConfigurator.SecretKey(awsSettings.SecretKey);
        });
        configurator.UsePublishMessageScheduler();
        configurator.ConfigureEndpoints(context);

        configurator.UseNewtonsoftJsonSerializer();
        configurator.UseNewtonsoftJsonDeserializer();

        EndpointConvention.Map<SubmitOrder>(new Uri(EndpointAddresses.Ordering.SubmitOrder));
        EndpointConvention.Map<DeleteBuyerByEmail>(new Uri(EndpointAddresses.Identity.DeleteBuyer));
        EndpointConvention.Map<UpdateBuyer>(new Uri(EndpointAddresses.Identity.UpdateBuyer));
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

await app.RunAsync();