using System.Threading;
using System.Threading.Tasks;
using AutoBogus;
using CoffeeSpace.AClient.Messages.Commands;
using CoffeeSpace.AClient.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mediator;
using SukiUI.Controls;

namespace CoffeeSpace.AClient.ViewModels;

public sealed partial class ProductDetailsViewModel : ViewModelBase
{
    [ObservableProperty]
    private Product _product = null!;

    private readonly ISender _sender;

    public ProductDetailsViewModel(ISender sender)
    {
        _sender = sender;
    }

    public ProductDetailsViewModel()
    {
        Product = AutoFaker.Generate<Product>();
        _sender = null!;
    }
        
    [RelayCommand]
    private async Task AddToCart(CancellationToken cancellationToken)
    {
        await _sender.Send(new AddToCartCommand
        {
            Product = Product
        }, cancellationToken);
        
        await SukiHost.ShowToast("Cart is updated", "Item has been added!");
        SukiHost.CloseDialog();
    }
}