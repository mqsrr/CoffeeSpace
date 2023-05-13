using CoffeeSpace.Application.Extensions;
using CoffeeSpace.Application.Settings;
using CoffeeSpace.PaymentService.Consumers;
using CoffeeSpace.PaymentService.Persistence;
using CoffeeSpace.PaymentService.Repositories;
using CoffeeSpace.PaymentService.Repositories.Abstractions;
using MassTransit;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddAzureKeyVault();

builder.Services.AddControllers();

builder.Services.AddNpgsql<PaymentDbContext>(builder.Configuration["PaymentDb:ConnectionString"]!);

builder.Services.AddScoped<IPaymentHistoryRepository, PaymentHistoryRepository>();

builder.Services.AddOptions<RabbitMqSettings>()
    .Bind(builder.Configuration.GetRequiredSection("RabbitMq"))
    .ValidateOnStart();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddConsumer<OrderPaymentValidationConsumer>();

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