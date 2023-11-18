using Asp.Versioning;
using CoffeeSpace.Core.Extensions;
using CoffeeSpace.Core.Services.Abstractions;
using CoffeeSpace.Core.Settings;
using CoffeeSpace.ProductApi.Application.Extensions;
using CoffeeSpace.ProductApi.Application.Messages.Consumers;
using CoffeeSpace.ProductApi.Application.Repositories;
using CoffeeSpace.ProductApi.Application.Repositories.Abstractions;
using CoffeeSpace.ProductApi.Application.Validators;
using CoffeeSpace.ProductApi.Persistence;
using CoffeeSpace.ProductApi.Persistence.Abstractions;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddAzureKeyVault();
builder.Configuration.AddJwtBearer(builder);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
        .AddDatadogLogging("Product API"));

builder.Services.AddControllers();
builder.Services.AddApiVersioning(new MediaTypeApiVersionReader("api-version"));

builder.Services.AddStackExchangeRedisCache(x => x.Configuration = builder.Configuration["Redis:ConnectionString"]);
builder.Services.AddApplicationDb<IProductDbContext , ProductDbContext>(builder.Configuration["ProductsDb:ConnectionString"]!);

builder.Services.AddApplicationService<IProductRepository>();

builder.Services.AddApplicationService(typeof(ICacheService<>));
builder.Services.Decorate<IProductRepository, CachedProductRepository>();

builder.Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>(ServiceLifetime.Singleton, includeInternalTypes: true);

builder.Services.AddOptions<AwsMessagingSettings>()
    .Bind(builder.Configuration.GetRequiredSection(AwsMessagingSettings.SectionName))
    .ValidateOnStart();

builder.Services.AddOptions<JwtSettings>()
    .Bind(builder.Configuration.GetRequiredSection(JwtSettings.SectionName))
    .ValidateOnStart();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddConsumer<OrderStockValidationConsumer>();

    x.AddInMemoryInboxOutbox();
    x.UsingAmazonSqs((context, configurator) =>
    {
        var awsSettings = context.GetRequiredService<IOptions<AwsMessagingSettings>>().Value;
        configurator.Host(awsSettings.Region, hostConfigurator =>
        {
            hostConfigurator.AccessKey(awsSettings.AccessKey);
            hostConfigurator.SecretKey(awsSettings.SecretKey);
        });
        
        configurator.ConfigureEndpoints(context);
        
        configurator.UseNewtonsoftJsonSerializer();
        configurator.UseNewtonsoftJsonDeserializer();
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