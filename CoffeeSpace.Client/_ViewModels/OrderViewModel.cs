using System.Collections.ObjectModel;
using CoffeeSpace.Client.Models.Ordering;
using CoffeeSpace.Client.Services.Abstractions;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CoffeeSpace.Client._ViewModels;

public sealed partial class OrderViewModel : ObservableObject
{
    [ObservableProperty] 
    private ObservableCollection<Order> _orders;


    public OrderViewModel(ProfileViewModel profileViewModel, IHubConnectionService hubConnectionService)
    {
        _orders = profileViewModel.Buyer.Orders.ToObservableCollection();
        hubConnectionService.OrderCreated(order =>
        {
            bool isExists = _orders.Any(o => order.Id == o.Id);
            if (isExists)
            {
                return;
            }

            Orders.Add(order);
        });

        hubConnectionService.OrderStatusUpdated((newOrderStatus, orderId) =>
        {
            var orderToUpdate = Orders.FirstOrDefault(order => order.Id == orderId);
            if (orderToUpdate is null)
            {
                return;
            }

            Application.Current.Dispatcher.Dispatch(() =>
            {
                orderToUpdate.Status = newOrderStatus;
                Orders.Remove(orderToUpdate);
                Orders.Add(orderToUpdate);
            });

        });
    }
}