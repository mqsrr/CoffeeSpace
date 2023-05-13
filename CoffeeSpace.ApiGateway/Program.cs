using Azure.Identity;
using CoffeeSpace.ApiGateway.Extensions;
using CoffeeSpace.ApiGateway.Services;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Kubernetes;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("ocelot.json", false, true)
    .AddEnvironmentVariables()
    .AddAzureKeyVault()
    .AddJwtBearer(builder);

builder.Services
    .AddOcelot(builder.Configuration)
    .AddPolly()
    .AddKubernetes();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

await app.UseOcelot();

app.Run();