using System.Threading.RateLimiting;
using CoffeeSpace.Application.Extensions;
using CoffeeSpace.Application.Services.Abstractions;
using CoffeeSpace.Application.Settings;
using CoffeeSpace.ProductApi.Application.Extensions;
using CoffeeSpace.ProductApi.Application.Messages.Consumers;
using CoffeeSpace.ProductApi.Application.Repositories;
using CoffeeSpace.ProductApi.Application.Repositories.Abstractions;
using CoffeeSpace.ProductApi.Application.Services.Abstractions;
using CoffeeSpace.ProductApi.Application.Validators;
using CoffeeSpace.ProductApi.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddAzureKeyVault();

builder.Configuration.AddJwtBearer(builder);

builder.Services.AddControllers();
builder.Services.AddMediator();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    
    options.AddTokenBucketLimiter("TokenBucket", limiterOptions =>
    {
        limiterOptions.TokenLimit = 20;
        limiterOptions.ReplenishmentPeriod = TimeSpan.FromSeconds(5);
        limiterOptions.TokensPerPeriod = 5;
        limiterOptions.QueueLimit = 3;
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});

builder.Services.AddStackExchangeRedisCache(x => 
    x.Configuration = builder.Configuration["Redis:ConnectionString"]);

builder.Services.AddApplicationDb<IProductDbContext, ProductDbContext>(builder.Configuration["ProductsDb:ConnectionString"]!);

builder.Services.AddApplicationService<IProductRepository>();
builder.Services.AddApplicationService<IProductService>();

builder.Services.AddApplicationService(typeof(ICacheService<>));

builder.Services.Decorate<IProductRepository, CachedProductRepository>();

builder.Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>(ServiceLifetime.Singleton, includeInternalTypes: true);

builder.Services.AddOptions<RabbitMqSettings>()
    .Bind(builder.Configuration.GetRequiredSection("RabbitMq"))
    .ValidateOnStart();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddConsumer<AwaitProductsValidationConsumer>();
    
    x.UsingRabbitMq((context, config) =>
    {
        var rabbitMqSettings = context.GetRequiredService<IOptions<RabbitMqSettings>>().Value;
        config.Host(rabbitMqSettings.Host, "/", hostConfig =>
        {
            hostConfig.Username(rabbitMqSettings.Username);
            hostConfig.Password(rabbitMqSettings.Password);
        });
        
        config.UseNewtonsoftJsonSerializer();
        config.UseNewtonsoftJsonDeserializer();
        
        config.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();