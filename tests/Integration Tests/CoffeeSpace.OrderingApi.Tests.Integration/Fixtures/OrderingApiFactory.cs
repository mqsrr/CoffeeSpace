using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using AutoBogus;
using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.Messages;
using CoffeeSpace.Messages.Buyers;
using CoffeeSpace.Messages.Ordering.Commands;
using CoffeeSpace.OrderingApi.Application.Contracts.Responses.Orders;
using CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Consumers;
using CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Sagas;
using CoffeeSpace.OrderingApi.Application.SignalRHubs;
using CoffeeSpace.OrderingApi.Application.SignalRHubs.Abstraction;
using CoffeeSpace.OrderingApi.Controllers;
using CoffeeSpace.OrderingApi.Persistence;
using CoffeeSpace.OrderingApi.Tests.Integration.Fakers.Models;
using CoffeeSpace.Shared.Settings;
using DotNet.Testcontainers.Builders;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;
using Quartz;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;
using Xunit;

namespace CoffeeSpace.OrderingApi.Tests.Integration.Fixtures;

public sealed class OrderingApiFactory : WebApplicationFactory<BuyersController>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _orderingPostgreSqlContainer;
    private readonly RedisContainer _redisContainer;

    public IEnumerable<Buyer> Buyers { get; init; }
    public IEnumerable<Order> Orders { get; init; }
    
    public OrderingApiFactory()
    {
        _orderingPostgreSqlContainer = new PostgreSqlBuilder()
            .WithDatabase("OrdersDb")
            .WithUsername("test")
            .WithPassword("test")
            .WithPortBinding(5433, 5432)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
            .Build();

        _redisContainer = new RedisBuilder()
            .WithPortBinding(6378, 6379)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(6379))
            .Build();
        
        Buyers = AutoFaker.Generate<Buyer, BuyerFaker>(3);
        Orders = AutoFaker.Generate<Order, OrderFaker>(5, builder => builder.WithArgs(Buyers.First().Id));
        
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("OrderingDb:ConnectionString", _orderingPostgreSqlContainer.GetConnectionString());
        builder.UseSetting("Redis:ConnectionString", _redisContainer.GetConnectionString());

        builder.UseSetting("AWS:Region", "eu");
        builder.UseSetting("AWS:AccessKey", "test");
        builder.UseSetting("AWS:SecretKey", "test");
        
        builder.UseSetting("Jwt:Audience", "coffee-space.testing");
        builder.UseSetting("Jwt:Issuer", "coffee-space.testing");
        builder.UseSetting("Jwt:Key", "testing-coffeespac!!23LOOOOOONGKEYYY!!!!");
        builder.UseSetting("Jwt:Expire", "1");

        builder.UseSetting("Azure:SignalR:ConnectionString", "Endpoint=https://testing.service.signalr.net;AccessKey=testing-access-key;Version=1.0;");

        Environment.SetEnvironmentVariable(Environments.Staging, "Testing");
        builder.ConfigureTestServices(services =>
        {
            services.AddSignalR();
            services.RemoveAll(typeof(IHubContext<OrderingHub, IOrderingHub>));
            
            var hubContext = Substitute.For<IHubContext<OrderingHub, IOrderingHub>>();
            hubContext.Clients.Groups(Arg.Any<string[]>())
                .OrderCreated(Arg.Any<OrderResponse>())
                .Returns(Task.CompletedTask);
            
            services.AddScoped<IHubContext<OrderingHub, IOrderingHub>>(_ => hubContext);
            
            services.AddQuartz();
            services.AddMassTransitTestHarness(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();
                config.AddConsumer<RegisterNewBuyerConsumer>();
                
                config.AddQuartzConsumers();
                config.AddPublishMessageScheduler();

                config.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>()
                    .InMemoryRepository();
                
                EndpointConvention.Map<SubmitOrder>(new Uri(EndpointAddresses.Ordering.SubmitOrder));
                EndpointConvention.Map<DeleteBuyerByEmail>(new Uri(EndpointAddresses.Identity.DeleteBuyer));
                EndpointConvention.Map<UpdateBuyer>(new Uri(EndpointAddresses.Identity.UpdateBuyer));
            });
            
            services.AddScoped(_ => new DbContextOptionsBuilder<OrderingDbContext>()
                .EnableDetailedErrors()
                .UseNpgsql(_orderingPostgreSqlContainer.GetConnectionString())
                .Options);       
            
            InitializeOrderingDbContext(services);
        });
    }

    protected override void ConfigureClient(HttpClient client)
    {
        var jwtSettings = Services.GetRequiredService<IOptions<JwtSettings>>().Value;
        string token = WriteToken(jwtSettings);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
    
    public async Task InitializeAsync()
    {
        await _orderingPostgreSqlContainer.StartAsync();
        await _redisContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _orderingPostgreSqlContainer.StopAsync();
        await _redisContainer.StopAsync();
    }

    private static string WriteToken(JwtSettings jwtSettings)
    {
        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
        var signingCred = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);
        var jwtSecurityToken = new JwtSecurityToken(
            jwtSettings.Issuer,
            jwtSettings.Audience,
            [new Claim(ClaimTypes.System, "testing")],
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(jwtSettings.Expire),
            signingCred);

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }

    private void InitializeOrderingDbContext(IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider(true).CreateScope();
        var orderingDbContext = serviceProvider.ServiceProvider.GetRequiredService<OrderingDbContext>();

        orderingDbContext.Database.EnsureCreated();

        orderingDbContext.Buyers.AddRange(Buyers);
        orderingDbContext.Orders.AddRange(Orders);
        orderingDbContext.SaveChanges();
    }
}