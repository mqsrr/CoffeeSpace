using Asp.Versioning;
using CoffeeSpace.Application.Extensions;
using CoffeeSpace.Application.Settings;
using CoffeeSpace.IdentityApi.Extensions;
using CoffeeSpace.IdentityApi.Messages.Consumers;
using CoffeeSpace.IdentityApi.Models;
using CoffeeSpace.IdentityApi.Persistence;
using CoffeeSpace.IdentityApi.Services.Abstractions;
using CoffeeSpace.IdentityApi.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddAzureKeyVault();

builder.Services.AddControllers();

builder.Services.AddMediator();

builder.Services.AddBucketRateLimiter(StatusCodes.Status429TooManyRequests);

builder.Services.AddApiVersioning(new MediaTypeApiVersionReader("api-version"));

builder.Services.AddNpgsql<ApplicationUsersDbContext>(builder.Configuration["IdentityDb:ConnectionString"]);

builder.Services.AddApplicationService<IAuthService<ApplicationUser>>();
builder.Services.AddApplicationService<ITokenWriter<ApplicationUser>>();

builder.Services.AddOptions<JwtSettings>()
    .Bind(builder.Configuration.GetRequiredSection("Jwt"))
    .ValidateOnStart();

builder.Services.AddOptions<RabbitMqSettings>()
    .Bind(builder.Configuration.GetRequiredSection("RabbitMq"))
    .ValidateOnStart();

builder.Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<LoginRequestValidator>(ServiceLifetime.Singleton, includeInternalTypes: true);

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddConsumer<DeleteBuyerConsumer>();
    
    x.UsingRabbitMq((context, config) =>
    {
        var rabbitMqSettings = context.GetRequiredService<IOptions<RabbitMqSettings>>().Value;
        config.Host(rabbitMqSettings.Host, "/", hostConfig =>
        {
            hostConfig.Username(rabbitMqSettings.Username);
            hostConfig.Password(rabbitMqSettings.Password);
        });

        config.ConfigureEndpoints(context);
    });
});

builder.Services.AddIdentityConfiguration();

var app = builder.Build();

app.UseRateLimiter();

app.MapControllers();

app.Run();