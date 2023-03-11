using CoffeeSpace.Application.Context;
using CoffeeSpace.Application.Repositories.Interfaces;
using CoffeeSpace.WebAPI;
using CoffeeSpace.WebAPI.Extensions;
using CoffeeSpace.WebAPI.Filters;
using CoffeeSpace.WebAPI.MappingProfiles;
using CoffeeSpace.WebAPI.Options;
using CoffeeSpace.WebAPI.Services.Interfaces;
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
#pragma warning disable CS0618
    .AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<Program>());
#pragma warning restore CS0618

builder.Services.AddApplicationService(typeof(IValidator<>), typeof(Program).Assembly, ServiceLifetime.Scoped);
builder.Services.AddApplicationService(typeof(IRepository<>), typeof(IRepository<>).Assembly, ServiceLifetime.Scoped);

builder.Services.AddApplicationService(typeof(ITokenProvider<>), typeof(ITokenProvider<>).Assembly);
builder.Services.AddApplicationService(typeof(IAccountService), typeof(IAccountService).Assembly);

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<CustomerProfile>();
    config.AddProfile<OrderItemProfile>();
});

builder.Services.AddOptions<JwtOption>()
    .Bind(builder.Configuration.GetSection("Jwt"))
    .ValidateOnStart();

var app = builder.Build();

app.UseRouting();

app.MapControllers();

app.MapHub<OrderHub>("orders");

app.Run();
