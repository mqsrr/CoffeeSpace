using System.Collections.ObjectModel;
using CoffeeSpace.Data.Models.Orders;
using CoffeeSpace.Messages.Requests;
using CoffeeSpace.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;

namespace CoffeeSpace._ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<OrderItem> _products;

    private readonly IMediator _mediator;

    public MainViewModel(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [RelayCommand]
    private Task AddToCart(OrderItem item) => _mediator.Send(new AddToCartRequest(item));
}