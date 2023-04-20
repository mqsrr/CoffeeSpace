using CoffeeSpace.ProductApi.Application.Extensions;
using CoffeeSpace.ProductApi.Application.Messages.Consumers;
using CoffeeSpace.ProductApi.Application.Repositories;
using CoffeeSpace.ProductApi.Application.Repositories.Abstractions;
using CoffeeSpace.ProductApi.Application.Services.Abstractions;
using CoffeeSpace.ProductApi.Application.Settings;
using CoffeeSpace.ProductApi.Persistence;
using MassTransit;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddMediator();

builder.Services.AddStackExchangeRedisCache(x => 
    x.Configuration = builder.Configuration["Redis:ConnectionString"]);

builder.Services.AddApplicationDb<IProductDbContext, ProductDbContext>(builder.Configuration["ProductsDb:ConnectionString"]!);

builder.Services.AddApplicationService<IProductRepository>();
builder.Services.AddApplicationService<IProductService>();

builder.Services.AddApplicationService(typeof(ICacheService<>));

builder.Services.Decorate<IProductRepository, CachedProductRepository>();

builder.Services.AddOptions<RabbitMqSettings>()
    .Bind(builder.Configuration.GetSection("RabbitMq"))
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
        config.ConfigureServiceEndpoints(context);
    });
});

var app = builder.Build();

app.MapControllers();

app.Run();