using Asp.Versioning;
using CoffeeSpace.Core.Extensions;
using CoffeeSpace.Core.Settings;
using CoffeeSpace.IdentityApi.Application.Extensions;
using CoffeeSpace.IdentityApi.Application.Messages.Consumers;
using CoffeeSpace.IdentityApi.Application.Models;
using CoffeeSpace.IdentityApi.Application.Services.Abstractions;
using CoffeeSpace.IdentityApi.Application.Validators;
using CoffeeSpace.IdentityApi.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) => 
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Configuration.AddAzureKeyVault();

builder.Services.AddControllers();
builder.Services.AddMediator();

builder.Services.AddApiVersioning(new MediaTypeApiVersionReader("api-version"));

builder.Services.AddApplicationDb<ApplicationUsersDbContext>(builder.Configuration["IdentityDb:ConnectionString"]!);

builder.Services.AddApplicationService<IAuthService<ApplicationUser>>();
builder.Services.AddApplicationService<ITokenWriter<ApplicationUser>>();

builder.Services.AddOptions<JwtSettings>()
    .Bind(builder.Configuration.GetRequiredSection("Jwt"))
    .ValidateOnStart();

builder.Services.AddOptions<AwsMessagingSettings>()
    .Bind(builder.Configuration.GetRequiredSection("AWS"))
    .ValidateOnStart();

builder.Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<LoginRequestValidator>(ServiceLifetime.Singleton, includeInternalTypes: true);

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    
    x.AddConsumer<DeleteBuyerConsumer>();
    x.AddConsumer<UpdateBuyerConsumer>();
    
    x.UsingAmazonSqs((context, config) =>
    {
        var awsSettings = context.GetRequiredService<IOptions<AwsMessagingSettings>>().Value;
        config.Host(awsSettings.Region, hostConfig =>
        {
            hostConfig.AccessKey(awsSettings.AccessKey);
            hostConfig.SecretKey(awsSettings.SecretKey);
        });

        config.ConfigureEndpoints(context);
        
        config.UseNewtonsoftJsonSerializer();
        config.UseNewtonsoftJsonDeserializer();
    });
});

builder.Services.AddIdentityConfiguration();
builder.Services.AddServiceHealthChecks(builder);

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseHealthChecks("/_health");

app.MapControllers();

app.Run();