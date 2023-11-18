using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using AutoBogus;
using CoffeeSpace.Core.Settings;
using CoffeeSpace.Domain.Products;
using CoffeeSpace.ProductApi.Controllers;
using CoffeeSpace.ProductApi.Persistence;
using CoffeeSpace.ProductApi.Tests.Integration.Fakers;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;
using VerifyTests.EntityFramework;

namespace CoffeeSpace.ProductApi.Tests.Integration.Fixtures;

public class ProductApiFactory : WebApplicationFactory<ProductsController>, IAsyncLifetime
{
    private static IModel _dbModel = default!;

    private readonly PostgreSqlContainer _postgreSqlContainer;
    private readonly RedisContainer _redisContainer;

    public IEnumerable<Product> Products { get; init; }

    public ProductApiFactory()
    {
        _postgreSqlContainer = new PostgreSqlBuilder()
            .WithDatabase("ProductsDb")
            .WithUsername("test")
            .WithPassword("test")
            .WithPortBinding(5432, 5432)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
            .Build();

        _redisContainer = new RedisBuilder()
            .WithPortBinding(6379)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(6379))
            .Build();

        Products = AutoFaker.Generate<Product, ProductFaker>(10);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ProductsDb:ConnectionString", _postgreSqlContainer.GetConnectionString());
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
            services.AddScoped(_ => new DbContextOptionsBuilder<ProductDbContext>()
                .EnableRecording("ProductDb")
                .UseNpgsql(_postgreSqlContainer.GetConnectionString())
                .Options);

            using var serviceProvider = services.BuildServiceProvider(true).CreateScope();
            var dbContext = serviceProvider.ServiceProvider.GetRequiredService<ProductDbContext>();

            dbContext.Database.EnsureCreated();
            dbContext.Products.AddRange(Products);
            
            dbContext.SaveChanges();
            _dbModel = dbContext.Model;
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
    }
    
    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
        await _redisContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _postgreSqlContainer.StopAsync();
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
}