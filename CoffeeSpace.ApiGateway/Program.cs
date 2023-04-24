using CoffeeSpace.ApiGateway.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Docker;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("ocelot.json", false, true)
    .AddEnvironmentVariables();

builder.AddJwtBearer();

builder.Services
    .AddOcelot(builder.Configuration)
    .AddPolly()
    .AddDocker();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

await app.UseOcelot();

app.Run();