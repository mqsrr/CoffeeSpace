using System.Collections.ObjectModel;
using CoffeeSpace.Data.Models.Orders;
using CoffeeSpace.Messages.Requests;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;

namespace CoffeeSpace._ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<OrderItem> _products;

    private readonly ISender _sender;

    public MainViewModel(ISender sender) => _sender = sender;

    [RelayCommand]
    private Task AddToCart(OrderItem item, CancellationToken cancellationToken) 
        => _sender.Send(new AddToCartRequest(item), cancellationToken);
}