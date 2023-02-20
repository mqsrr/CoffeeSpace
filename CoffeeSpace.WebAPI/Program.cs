using CoffeeSpace.Data.Context;
using CoffeeSpace.WebAPI;
using CoffeeSpace.WebAPI.Extensions;
using CoffeeSpace.WebAPI.Filters;
using CoffeeSpace.WebAPI.MappingProfiles;
using CoffeeSpace.WebAPI.Options;
using CoffeeSpace.WebAPI.Services.Interfaces;
using CoffeeSpace.WebAPI.Services.Repository.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSignalR()
    .AddAzureSignalR(builder.Configuration["SignalR:ConnectionString"]);

builder.AddMySqlDbContext<ApplicationDb>("ApplicationDb:ConnectionString");
builder.AddMySqlDbContext<CustomersDb>("CustomersDb:ConnectionString");

builder.AddJwtBearer();

builder.Services.AddIdentityDb();

builder.Services.AddMvc(options => options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<Program>());

builder.Services.AddApplicationService(typeof(IValidator<>), ServiceLifetime.Scoped);
builder.Services.AddApplicationService(typeof(IRepository<>), ServiceLifetime.Scoped);

builder.Services.AddApplicationService(typeof(ITokenProvider<>), ServiceLifetime.Transient);
builder.Services.AddApplicationService(typeof(IAccountService), ServiceLifetime.Transient);

builder.Services.AddAutoMapper(config => 
    config.AddProfile(typeof(CustomerProfile)));

builder.Services.AddOptions<JwtOption>()
    .Bind(builder.Configuration.GetSection("Jwt"))
    .ValidateOnStart();

var app = builder.Build();

app.UseRouting();
app.UseFileServer();

app.MapControllers();

app.MapHub<OrderHub>("orders");

app.Run();
