using CoffeeSpace.Client._ViewModels;
using CoffeeSpace.Client.Services.Abstractions;

namespace CoffeeSpace.Client.Views;

public partial class OrderView
{
    private readonly IHubConnectionService _hubConnectionService;

    public OrderView(OrderViewModel viewModel, IHubConnectionService hubConnectionService)
    {
        InitializeComponent();
        
        BindingContext = viewModel;
        _hubConnectionService = hubConnectionService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        bool isHubConnected = _hubConnectionService.IsConnected;
        if (isHubConnected)
        {
            return;
        }

        string buyerId = await SecureStorage.GetAsync("buyer-id");
        await _hubConnectionService.StartConnectionAsync(buyerId, CancellationToken.None);
    }
}