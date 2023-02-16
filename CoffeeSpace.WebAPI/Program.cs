using CoffeeSpace.Data.Context;
using CoffeeSpace.Data.Models.CustomerInfo;
using CoffeeSpace.Data.Models.Orders;
using CoffeeSpace.WebAPI;
using CoffeeSpace.WebAPI.Services.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR()
    .AddAzureSignalR(builder.Configuration["SignalR:ConnectionString"]);

builder.Services.AddEndpointsApiExplorer();

string applicationConnectionString = builder.Configuration["ApplicationDb:ConnectionString"]!;
string customersConnectionString = builder.Configuration["CustomersDb:ConnectionString"]!;

builder.Services.AddMySql<ApplicationDb>(applicationConnectionString, ServerVersion.AutoDetect(applicationConnectionString));
builder.Services.AddMySql<CustomersDb>(customersConnectionString, ServerVersion.AutoDetect(customersConnectionString));

builder.Services.AddIdentity<Customer, IdentityRole>()
    .AddEntityFrameworkStores<CustomersDb>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IRepository<OrderItem>, OrderItemRepository>();
builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseRouting();
app.UseFileServer();

app.MapControllers();

app.MapHub<OrderHub>("orders");

app.Run();