using CoffeeSpace.OrderingApi.Application.Extensions;
using CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Sagas;
using CoffeeSpace.OrderingApi.Application.Repositories.Abstractions;
using CoffeeSpace.OrderingApi.Application.Services.Abstractions;
using CoffeeSpace.OrderingApi.Application.Settings;
using CoffeeSpace.OrderingApi.Persistence;
using CoffeeSpace.OrderingApi.Persistence.Abstractions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplicationDb<IOrderingDbContext, OrderingDbContext>(builder.Configuration["OrderingDb:ConnectionString"]!);

builder.Services.AddApplicationService(typeof(ICacheService<>));

builder.Services.AddApplicationService<IOrderService>();
builder.Services.AddApplicationService<IOrderRepository>();

builder.Services.AddApplicationService<IBuyerService>();
builder.Services.AddApplicationService<IBuyerRepository>();

builder.Services.AddOptions<RabbitMqSettings>()
    .Bind(builder.Configuration.GetSection("RabbitMq"))
    .ValidateOnStart();

builder.Services.AddDbContextOptions<OrderStateSagaDbContext>(builder.Configuration["OrderStateSagaDb:ConnectionString"]!);

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    
    x.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>()
        .EntityFrameworkRepository(configurator =>
        {
            configurator.ConcurrencyMode = ConcurrencyMode.Pessimistic;

            configurator.UseMySql();
            configurator.AddDbContext<DbContext, OrderStateSagaDbContext>((services, optionsBuilder) =>
            {
                var dbSettings = services.GetRequiredService<IOptions<DbContextSettings<OrderStateSagaDbContext>>>().Value;
                optionsBuilder.UseMySql(dbSettings.ConnectionString, dbSettings.ServerVersion);
            });
        });
    
    x.UsingRabbitMq((context, config) =>
    {
        var rabbitMqSettings = context.GetRequiredService<IOptions<RabbitMqSettings>>().Value;
        config.Host(rabbitMqSettings.Host, "/", hostConfig =>
        {
            hostConfig.Username(rabbitMqSettings.Username);
            hostConfig.Password(rabbitMqSettings.Password);
        });
        
        config.UseNewtonsoftJsonSerializer();
        config.UseNewtonsoftJsonSerializer();
        
        config.ConfigureEndpoints(context);
    });
});

builder.Services.AddMediator();

builder.Services.AddStackExchangeRedisCache(x =>
    x.Configuration = builder.Configuration["Redis:ConnectionString"]!);

var app = builder.Build();


app.MapControllers();

app.Run();