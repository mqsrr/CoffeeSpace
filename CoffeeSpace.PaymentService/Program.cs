using CoffeeSpace.PaymentService.Application.Consumers;
using CoffeeSpace.PaymentService.Application.Repositories;
using CoffeeSpace.PaymentService.Application.Repositories.Abstractions;
using CoffeeSpace.PaymentService.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<PaymentDbContext>(options => 
    options.UseNpgsql(builder.Configuration["PaymentDb:ConnectionString"]!));

builder.Services.AddScoped<IPaymentHistoryRepository, PaymentHistoryRepository>();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddConsumer<OrderPaymentValidationConsumer>();

    x.UsingRabbitMq((context, config) =>
    {
        var rabbitMqSettings = builder.Configuration.GetRequiredSection("RabbitMq");
        config.Host(rabbitMqSettings["Host"], "/", hostConfig =>
        {
            hostConfig.Username(rabbitMqSettings["Username"]);
            hostConfig.Password(rabbitMqSettings["Password"]);
        });
        config.UseNewtonsoftJsonSerializer();
        config.UseNewtonsoftJsonDeserializer();

        config.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.MapControllers();

app.Run();