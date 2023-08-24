using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using AutoBogus;
using CoffeeSpace.Core.Settings;
using CoffeeSpace.Domain.Ordering.BuyerInfo;
using CoffeeSpace.Domain.Ordering.BuyerInfo.CardInfo;
using CoffeeSpace.Domain.Ordering.Orders;
using CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Consumers;
using CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Sagas;
using CoffeeSpace.OrderingApi.Application.Messaging.Masstransit.Sagas.Definitions;
using CoffeeSpace.OrderingApi.Controllers;
using CoffeeSpace.OrderingApi.Persistence;
using CoffeeSpace.OrderingApi.Tests.Integration.Fakers;
using CoffeeSpace.OrderingApi.Tests.Integration.Fakers.Models;
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
using Microsoft.IdentityModel.Tokens;
using Quartz;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;
using VerifyTests.EntityFramework;

namespace CoffeeSpace.OrderingApi.Tests.Integration.Fixtures;

public sealed class OrderingApiFactory : WebApplicationFactory<BuyersController>, IAsyncLifetime
{
    private static IModel _dbModel = default!;

    private readonly PostgreSqlContainer _orderingPostgreSqlContainer;
    private readonly PostgreSqlContainer _orderStateSagaPostgreSqlContainer;
    private readonly RedisContainer _redisContainer;

    public IEnumerable<Address> Addresses { get; init; }
    public IEnumerable<PaymentInfo> PaymentInfos { get; init; }
    public IEnumerable<Buyer> Buyers { get; init; }
    public IEnumerable<OrderItem> OrderItems { get; init; }
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
        
        _orderStateSagaPostgreSqlContainer = new PostgreSqlBuilder()
            .WithDatabase("OrderStateSagaDb")
            .WithUsername("test")
            .WithPassword("test")
            .WithPortBinding(5434, 5432)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
            .Build();

        _redisContainer = new RedisBuilder()
            .WithPortBinding(6378, 6379)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(6379))
            .Build();

        OrderItems = AutoFaker.Generate<OrderItem, OrderItemFaker>(10);
        Addresses = AutoFaker.Generate<Address, AddressFaker>(2);
        PaymentInfos = AutoFaker.Generate<PaymentInfo, PaymentInfoFaker>(2);
        Buyers = AutoFaker.Generate<Buyer, BuyerFaker>(3);
        
        Orders = AutoFaker.Generate<Order, OrderFaker>( 1, builder => 
            builder.WithArgs(Buyers.First().Id, Addresses.First(), PaymentInfos.First(), OrderItems.Take(3)));
        Orders = Orders.Append(AutoFaker.Generate<Order, OrderFaker>(2, builder =>
            builder.WithArgs(Buyers.First().Id, Addresses.Last(), PaymentInfos.Last(), OrderItems.Take(3))).Last());
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("OrderingDb:ConnectionString", _orderingPostgreSqlContainer.GetConnectionString());
        builder.UseSetting("OrderStateSagaDb:ConnectionString", _orderStateSagaPostgreSqlContainer.GetConnectionString());
        builder.UseSetting("Redis:ConnectionString", _redisContainer.GetConnectionString());

        builder.UseSetting("AWS:Region", "eu");
        builder.UseSetting("AWS:AccessKey", "test");
        builder.UseSetting("AWS:SecretKey", "test");

        builder.UseSetting("Jwt:Audience", "coffee-space.testing");
        builder.UseSetting("Jwt:Issuer", "coffee-space.testing");
        builder.UseSetting("Jwt:Key", "testing-coffeespac!!23");
        builder.UseSetting("Jwt:Expire", "1");

        Environment.SetEnvironmentVariable(Environments.Staging, "Testing");
        builder.ConfigureTestServices(services =>
        {
            services.AddQuartz(config => config.UseMicrosoftDependencyInjectionJobFactory());
            services.AddMassTransitTestHarness(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();
                config.AddConsumer<RegisterNewBuyerConsumer>();
                config.AddConsumer<UpdateOrderStatusConsumer>();

                config.AddQuartzConsumers();
                config.AddPublishMessageScheduler();

                config.AddEntityFrameworkOutbox<OrderStateSagaDbContext>(configurator =>
                {
                    configurator.IsolationLevel = IsolationLevel.ReadCommitted;        
                    configurator.UsePostgres();
                });
     
                config.AddSagaStateMachine<OrderStateMachine, OrderStateInstance, OrderStateDefinition>()
                    .EntityFrameworkRepository(configurator =>
                    {
                        configurator.ConcurrencyMode = ConcurrencyMode.Optimistic;
            
                        configurator.UsePostgres();
                        configurator.ExistingDbContext<OrderStateSagaDbContext>();
                    });
            });
            
            services.AddScoped(_ => new DbContextOptionsBuilder<OrderingDbContext>()
                .EnableRecording("OrderingDb")
                .EnableDetailedErrors()
                .UseNpgsql(_orderingPostgreSqlContainer.GetConnectionString())
                .Options);       
            
            services.AddScoped(_ => new DbContextOptionsBuilder<OrderStateSagaDbContext>()
                .EnableRecording("OrderStateSagaDb")
                .EnableDetailedErrors()
                .UseNpgsql(_orderStateSagaPostgreSqlContainer.GetConnectionString())
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
    
    [ModuleInitializer]
    public static void InitializeVerify()
    {
        VerifyEntityFramework.Initialize(_dbModel);
        VerifyMassTransit.Initialize();
    }
    
    public async Task InitializeAsync()
    {
        await _orderingPostgreSqlContainer.StartAsync();
        await _orderStateSagaPostgreSqlContainer.StartAsync();
        await _redisContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _orderingPostgreSqlContainer.StopAsync();
        await _orderStateSagaPostgreSqlContainer.StopAsync();
        await _redisContainer.StopAsync();
    }

    private static string WriteToken(JwtSettings jwtSettings)
    {
        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
        var signingCred = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);
        var jwtSecurityToken = new JwtSecurityToken(
            jwtSettings.Issuer,
            jwtSettings.Audience,
            new[] {new Claim(ClaimTypes.System, "testing")},
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(jwtSettings.Expire),
            signingCred);

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }

    private void InitializeOrderingDbContext(IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider(true).CreateScope();
        var orderingDbContext = serviceProvider.ServiceProvider.GetRequiredService<OrderingDbContext>();
        var orderStateSagaDbContext = serviceProvider.ServiceProvider.GetRequiredService<OrderStateSagaDbContext>();

        orderingDbContext.Database.EnsureCreated();
        orderStateSagaDbContext.Database.EnsureCreated();
            
        orderingDbContext.OrderItems.AddRange(OrderItems);
        orderingDbContext.Addresses.AddRange(Addresses);
        orderingDbContext.PaymentInfos.AddRange(PaymentInfos);
        orderingDbContext.SaveChanges();

        orderingDbContext.Buyers.AddRange(Buyers);
        orderingDbContext.Orders.AddRange(Orders);
        orderingDbContext.SaveChanges();
        
        _dbModel = orderingDbContext.Model;
    }
}