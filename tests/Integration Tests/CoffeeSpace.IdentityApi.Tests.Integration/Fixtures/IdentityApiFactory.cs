﻿using CoffeeSpace.IdentityApi.Application.Messages.Consumers;
using CoffeeSpace.IdentityApi.Controllers;
using CoffeeSpace.IdentityApi.Persistence;
using CoffeeSpace.IdentityApi.Settings;
using CoffeeSpace.Messages;
using CoffeeSpace.Messages.Buyers;
using DotNet.Testcontainers.Builders;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Testcontainers.PostgreSql;
using Xunit;

namespace CoffeeSpace.IdentityApi.Tests.Integration.Fixtures;

public sealed class IdentityApiFactory : WebApplicationFactory<AuthController>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _identityPostgreSqlContainer;
    
    public IdentityApiFactory()
    {
        _identityPostgreSqlContainer = new PostgreSqlBuilder()
            .WithDatabase("IdentityDb")
            .WithUsername("test")
            .WithPassword("test")
            .WithPortBinding(5435, 5432)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("IdentityDb:ConnectionString", _identityPostgreSqlContainer.GetConnectionString());

        builder.UseSetting("AWS:Region", "eu");
        builder.UseSetting("AWS:AccessKey", "test");
        builder.UseSetting("AWS:SecretKey", "test");
        
        builder.UseSetting("Jwt:Audience", "coffee-space.testing");
        builder.UseSetting("Jwt:Issuer", "coffee-space.testing");
        builder.UseSetting("Jwt:Key", "testing-coffeespac!!23LOOOOOONGKEY11111!!!!!!!!!!!!");
        builder.UseSetting("Jwt:Expire", "1");
        
        builder.UseSetting("Authorization:ApiKey", "1sdfsdfsdfsdfsdfdsfs");
        builder.UseSetting("Authorization:HeaderName", "X-Api-Key");

        Environment.SetEnvironmentVariable(Environments.Staging, "Testing");
        builder.ConfigureTestServices(services =>
        {
            services.AddMassTransitTestHarness(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();
    
                config.AddConsumer<DeleteBuyerConsumer>();
                config.AddConsumer<UpdateBuyerConsumer>();
                
                EndpointConvention.Map<RegisterNewBuyer>(new Uri(EndpointAddresses.Identity.RegisterNewBuyer));
            });

            services.AddScoped(_ => new DbContextOptionsBuilder<ApplicationUsersDbContext>()
                .EnableDetailedErrors()
                .UseNpgsql(_identityPostgreSqlContainer.GetConnectionString())
                .Options);

            using var serviceProvider = services.BuildServiceProvider(true).CreateScope();
            var dbContext = serviceProvider.ServiceProvider.GetRequiredService<ApplicationUsersDbContext>();

            dbContext.Database.EnsureCreated();
        });
    }

    protected override void ConfigureClient(HttpClient client)
    {
        var apiKeySettings = Services.GetRequiredService<IOptions<ApiKeySettings>>().Value;
        client.DefaultRequestHeaders.Add(apiKeySettings.HeaderName, apiKeySettings.ApiKey);
    }
    
    public async Task InitializeAsync()
    {
        await _identityPostgreSqlContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _identityPostgreSqlContainer.StopAsync();
    }
}