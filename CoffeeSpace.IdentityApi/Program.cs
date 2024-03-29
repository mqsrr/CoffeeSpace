using Asp.Versioning;
using CoffeeSpace.IdentityApi.Application.Extensions;
using CoffeeSpace.IdentityApi.Application.Messages.Consumers;
using CoffeeSpace.IdentityApi.Application.Models;
using CoffeeSpace.IdentityApi.Application.Services.Abstractions;
using CoffeeSpace.IdentityApi.Application.Validators;
using CoffeeSpace.IdentityApi.Filters;
using CoffeeSpace.IdentityApi.Persistence;
using CoffeeSpace.IdentityApi.Settings;
using CoffeeSpace.Messages.Buyers;
using CoffeeSpace.Shared.Extensions;
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

builder.Services.AddControllers();

builder.Services.AddApiVersioning(new HeaderApiVersionReader());
builder.Services.AddApplicationDb<ApplicationUsersDbContext>(builder.Configuration["IdentityDb:ConnectionString"]!);

builder.Services.AddApplicationService<IAuthService<ApplicationUser>>();
builder.Services.AddApplicationService<ITokenWriter<ApplicationUser>>();

builder.Services.AddApplicationServiceAsSelf<ApiKeyAuthorizationFilter>();

builder.Services.AddOptionsWithValidateOnStart<JwtSettings>()
    .Bind(builder.Configuration.GetRequiredSection(JwtSettings.SectionName));

builder.Services.AddOptionsWithValidateOnStart<ApiKeySettings>()
    .Bind(builder.Configuration.GetRequiredSection(ApiKeySettings.SectionName));

builder.Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<LoginRequestValidator>(ServiceLifetime.Singleton, includeInternalTypes: true);

builder.Services.AddMassTransit(x =>
{
    x.UsingInMemory();
    x.AddRider(configurator =>
    {
        configurator.AddProducer<RegisterNewBuyer>("register-customer");

        configurator.AddConsumer<DeleteBuyerConsumer>();
        configurator.AddConsumer<UpdateBuyerConsumer>();

        configurator.UsingKafka((context, kafkaConfigurator) =>
        {
            kafkaConfigurator.Acks = Acks.All;
            kafkaConfigurator.Host(builder.Configuration["Kafka:Host"]);
            
            kafkaConfigurator
                .AddTopicEndpoint<DeleteBuyer, DeleteBuyerConsumer>(context, "delete-customer", "identity")
                .AddTopicEndpoint<UpdateBuyer, UpdateBuyerConsumer>(context,"update-customer", "identity");
        });
    });
});

builder.Services.AddIdentityConfiguration();
builder.Services.AddServiceHealthChecks(builder);

var app = builder.Build();

app.UseSerilogRequestLogging();
app.MapPrometheusScrapingEndpoint();

app.UseHealthChecks("/_health");

app.MapControllers();
app.Run();