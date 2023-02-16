using System.Reflection;
using CoffeeSpace.Data.Models.CustomerInfo;
using CoffeeSpace.Data.Models.Orders;
using CoffeeSpace.Extensions;
using CoffeeSpace.Initializers;
using CoffeeSpace.Services;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;
using MediatR;
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

		builder.Services.AddScoped<IServiceDataProvider<Order>, ServiceDataProvider<Order>>();
		builder.Services.AddScoped<IServiceDataProvider<OrderItem>, ServiceDataProvider<OrderItem>>();
		builder.Services.AddScoped<IServiceDataProvider<Customer>, ServiceDataProvider<Customer>>();
			
		builder.Services.AddAuth0Client();
		builder.Services.AddOidClient();
		builder.Services.AddSignalRHubConnection();

		builder.Services.AddTransient<HttpClient>();
		builder.Services.AddTransient<IMauiInitializeService, MainViewInitializer>();
		
#if DEBUG
		builder.Logging.AddDebug();
#endif
		return builder.Build();
	}
}
