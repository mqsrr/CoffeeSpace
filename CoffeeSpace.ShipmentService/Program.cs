using CoffeeSpace.ShipmentService.Consumers;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddConsumer<RequestOrderShipmentConsumer>();
    
    x.UsingRabbitMq((context, config) =>
    {
        var rabbitSection = builder.Configuration.GetRequiredSection("RabbitMq");
        config.Host(rabbitSection["Host"], "/", hostConfig =>
        {
            hostConfig.Username(rabbitSection["Username"]);
            hostConfig.Password(rabbitSection["Password"]);
        });

        config.UseNewtonsoftJsonSerializer();
        config.UseNewtonsoftJsonDeserializer();

        config.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.MapControllers();

app.Run();