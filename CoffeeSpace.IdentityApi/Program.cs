using System.Threading.RateLimiting;
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
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddAzureKeyVault();

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

builder.Services.AddNpgsql<ApplicationUsersDbContext>("Server=localhost;Port=5433;Database=IdentityDb;User Id=sa;Password=identity-manager;");

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