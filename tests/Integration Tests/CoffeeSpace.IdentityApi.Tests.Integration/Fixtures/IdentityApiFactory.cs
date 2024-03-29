using System.Runtime.CompilerServices;
using CoffeeSpace.IdentityApi.Application.Messages.Consumers;
using CoffeeSpace.IdentityApi.Controllers;
using CoffeeSpace.IdentityApi.Persistence;
using CoffeeSpace.IdentityApi.Settings;
using DotNet.Testcontainers.Builders;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Testcontainers.Kafka;
using Testcontainers.PostgreSql;

namespace CoffeeSpace.IdentityApi.Tests.Integration.Fixtures;

public sealed class IdentityApiFactory : WebApplicationFactory<AuthController>, IAsyncLifetime
{
    private static IModel _dbModel = default!;

    private readonly PostgreSqlContainer _identityPostgreSqlContainer;
    private readonly KafkaContainer _kafkaContainer;
    
    public IdentityApiFactory()
    {
        _identityPostgreSqlContainer = new PostgreSqlBuilder()
            .WithDatabase("IdentityDb")
            .WithUsername("test")
            .WithPassword("test")
            .WithPortBinding(5435, 5432)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
            .Build();

        _kafkaContainer = new KafkaBuilder()
            .WithPortBinding(9092, 9092)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(9092))
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("IdentityDb:ConnectionString", _identityPostgreSqlContainer.GetConnectionString());
        builder.UseSetting("Kafka:ConnectionString", _kafkaContainer.GetBootstrapAddress());

        builder.UseSetting("Jwt:Audience", "coffee-space.testing");
        builder.UseSetting("Jwt:Issuer", "coffee-space.testing");
        builder.UseSetting("Jwt:Key", "testing-coffeespac!!23LOOOOOONGKEY11111!!!!!!!!!!!!");
        builder.UseSetting("Jwt:Expire", "1");
        
        builder.UseSetting("Authorization:ApiKey", "1sdfsdfsdfsdfsdfdsfs");
        builder.UseSetting("Authorization:HeaderName", "apiiii");

        Environment.SetEnvironmentVariable(Environments.Staging, "Testing");
        builder.ConfigureTestServices(services =>
        {
            services.AddMassTransitTestHarness(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();
    
                config.AddConsumer<DeleteBuyerConsumer>();
                config.AddConsumer<UpdateBuyerConsumer>();
            });

            services.AddScoped(_ => new DbContextOptionsBuilder<ApplicationUsersDbContext>()
                .EnableRecording("IdentityDb")
                .EnableDetailedErrors()
                .UseNpgsql(_identityPostgreSqlContainer.GetConnectionString())
                .Options);

            using var serviceProvider = services.BuildServiceProvider(true).CreateScope();
            var dbContext = serviceProvider.ServiceProvider.GetRequiredService<ApplicationUsersDbContext>();

            dbContext.Database.EnsureCreated();
            _dbModel = dbContext.Model;
        });
    }

    protected override void ConfigureClient(HttpClient client)
    {
        var apiKeySettings = Services.GetRequiredService<IOptions<ApiKeySettings>>().Value;
        client.DefaultRequestHeaders.Add(apiKeySettings.HeaderName, apiKeySettings.ApiKey);
    }

    [ModuleInitializer]
    public static void InitializeVerify()
    {
        VerifyEntityFramework.Initialize(_dbModel);
        VerifyMassTransit.Initialize();
    }

    public async Task InitializeAsync()
    {
        await _identityPostgreSqlContainer.StartAsync();
        await _kafkaContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _identityPostgreSqlContainer.StopAsync();
        await _kafkaContainer.StopAsync();
    }
}