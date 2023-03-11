using CoffeeSpace._ViewModels;
using CommunityToolkit.Maui;

namespace CoffeeSpace.Extensions;

public static class RegisterViewModelExtension
{
    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<OrderViewModel>();
        builder.Services.AddSingleton<CartViewModel>();
        builder.Services.AddSingleton<ProfileViewModel>();
        
        return builder;
    }
}
