using CoffeeSpace.Application.Extensions;
using CoffeeSpace.Application.Settings;
using CoffeeSpace.ShipmentService.Consumers;
using MassTransit;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddAzureKeyVault();

builder.Services.AddControllers();

builder.Services.AddOptions<RabbitMqSettings>()
    .Bind(builder.Configuration.GetRequiredSection("RabbitMq"))
    .ValidateOnStart();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddConsumer<RequestOrderShipmentConsumer>();
    
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

app.MapControllers();

app.Run();