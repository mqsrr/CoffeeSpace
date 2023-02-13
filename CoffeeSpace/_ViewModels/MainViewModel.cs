using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using CoffeeSpace.Data.Models.Orders;
using CoffeeSpace.Messages.Requests;
using CoffeeSpace.Services.Repository;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;

namespace CoffeeSpace._ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<OrderItem> _products;

    private readonly IRepository<OrderItem> _orderItemRepo;
    private readonly IMediator _mediator;

    public MainViewModel(IRepository<OrderItem> orderItemRepo, IMediator mediator)
    {
        _orderItemRepo = orderItemRepo;
        _mediator = mediator;
        
        _products = new ObservableCollection<OrderItem>(_orderItemRepo.GetAll());
        
        // _products = new ObservableCollection<OrderItem>
        // {
        //     new OrderItem
        //     {
        //         Title = "Hello ",
        //         PictureUrl = "coffee-latte.jpg",
        //         UnitPrice = 1.5m,
        //         Quantity= 1
        //     },
        //     new OrderItem
        //     {
        //         Title = "Hello 2",
        //         PictureUrl = "coffee-latte.jpg",
        //         UnitPrice = 2.5m,
        //         Quantity= 1
        //     }
        // };
    }
    
    [RelayCommand]
    private Task AddToCart(OrderItem item) => _mediator.Send(new AddToCartRequest(item));
}