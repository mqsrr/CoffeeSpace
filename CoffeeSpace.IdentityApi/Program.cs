using Asp.Versioning;
using CoffeeSpace.IdentityApi.Application.Extensions;
using CoffeeSpace.IdentityApi.Application.Messages.Consumers;
using CoffeeSpace.IdentityApi.Application.Models;
using CoffeeSpace.IdentityApi.Application.Services.Abstractions;
using CoffeeSpace.IdentityApi.Application.Validators;
using CoffeeSpace.IdentityApi.Filters;
using CoffeeSpace.IdentityApi.Persistence;
using CoffeeSpace.IdentityApi.Settings;
using CoffeeSpace.Messages.Buyers;
using CoffeeSpace.Shared.Extensions;
using CoffeeSpace.Shared.Settings;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddOpenTelemetryWithInstrumentation();

builder.Host.UseSerilog((context, configuration) => 
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Configuration.AddAzureKeyVault();
builder.Configuration.AddJwtBearer(builder);

builder.Services.AddControllers();

builder.Services.AddApiVersioning(new HeaderApiVersionReader());
builder.Services.AddApplicationDb<ApplicationUsersDbContext>("Server=localhost;Port=5433;Database=testDb;User Id=test;Password=Test1234!;");

builder.Services.AddApplicationService<IAuthService<ApplicationUser>>(ServiceLifetime.Transient);
builder.Services.AddApplicationService<ITokenWriter<ApplicationUser>>(ServiceLifetime.Transient);

builder.Services.AddApplicationServiceAsSelf<ApiKeyAuthorizationFilter>();

builder.Services.AddOptionsWithValidateOnStart<JwtSettings>()
    .Bind(builder.Configuration.GetRequiredSection(JwtSettings.SectionName));

builder.Services.AddOptionsWithValidateOnStart<ApiKeySettings>()
    .Bind(builder.Configuration.GetRequiredSection(ApiKeySettings.SectionName));

builder.Services.AddOptionsWithValidateOnStart<AwsMessagingSettings>()
    .Bind(builder.Configuration.GetRequiredSection(AwsMessagingSettings.SectionName));

builder.Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<LoginRequestValidator>(ServiceLifetime.Transient, includeInternalTypes: true);

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
        
        EndpointConvention.Map<RegisterNewBuyer>(new Uri("queue:register-new-buyer"));
    });
});
builder.Services.AddIdentityConfiguration();
builder.Services.AddServiceHealthChecks(builder);

var app = builder.Build();

app.UseSerilogRequestLogging();
app.MapPrometheusScrapingEndpoint();

app.UseHealthChecks("/_health");

app.MapControllers();
app.Run();