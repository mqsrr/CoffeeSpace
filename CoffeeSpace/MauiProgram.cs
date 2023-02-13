using System.Reflection;
using CoffeeSpace.Data.Context;
using CoffeeSpace.Data.Models.CustomerInfo;
using CoffeeSpace.Data.Models.Orders;
using CoffeeSpace.Extensions;
using CoffeeSpace.Services.Repository;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CoffeeSpace;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.UseMauiCommunityToolkitMarkup()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.RegisterViews()
			.RegisterViewModels()
			.ConfigureImageSources();

		builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

		builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());
		
		string applicationConnectionString = builder.Configuration["ApplicationDb:ConnectionString"];
		string customersConnectionString = builder.Configuration["CustomersDb:ConnectionString"];

		builder.Services.AddMySql<ApplicationDb>(applicationConnectionString, ServerVersion.AutoDetect(applicationConnectionString));
		builder.Services.AddMySql<CustomersDb>(customersConnectionString, ServerVersion.AutoDetect(customersConnectionString));

		builder.Services.AddIdentity<Customer, IdentityRole>()
			.AddEntityFrameworkStores<CustomersDb>()
			.AddDefaultTokenProviders();

		builder.Services.AddScoped<IRepository<OrderItem>, OrderItemRepository>();
		builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();
		builder.Services.AddScoped<IOrderRepository, OrderRepository>();

		builder.Services.AddAuth0Client();
		builder.Services.AddOidClient();
		builder.Services.AddSignalRHubConnection();

#if DEBUG
		builder.Logging.AddDebug();
#endif
		return builder.Build();
	}
}
