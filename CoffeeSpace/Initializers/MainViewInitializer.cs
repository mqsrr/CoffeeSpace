using System.Collections.ObjectModel;
using CoffeeSpace._ViewModels;
using CoffeeSpace.Application.Models.Orders;
using CoffeeSpace.Services;

namespace CoffeeSpace.Initializers;

public sealed class MainViewInitializer : IMauiInitializeService
{
    public async void Initialize(IServiceProvider services)
    {
        var provider = services.GetRequiredService<IOrderItemService>();
        var viewModel = services.GetRequiredService<MainViewModel>();

        var orderItems = await provider.GetAllAsync();

        viewModel.Products = new ObservableCollection<OrderItem>(orderItems);
    }
}