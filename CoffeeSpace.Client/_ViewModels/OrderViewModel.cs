using System.Collections.ObjectModel;
using CoffeeSpace.Client.Models.Ordering;
using CoffeeSpace.Client.Services.Abstractions;
using CoffeeSpace.Client.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoffeeSpace.Client._ViewModels;

public sealed partial class OrderViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Order> _orders;

    public OrderViewModel(IHubConnectionService hubConnectionService)
    {
        Orders = new ObservableCollection<Order>();

        hubConnectionService.OnOrderCreated(order =>
        {
            Application.Current!.Dispatcher.Dispatch(() =>
            {
                bool isContains = Orders.Any(o => o.Id == order.Id);
                if (isContains)
                {
                    return;
                }

                Orders.Add(order);
            });
        });

        hubConnectionService.OnOrderStatusUpdated((newOrderStatus, orderId) =>
        {
            var orderToUpdate = Orders.FirstOrDefault(order => order.Id == orderId);
            if (orderToUpdate is null)
            {
                return;
            }

            orderToUpdate.Status = newOrderStatus;
            if (newOrderStatus is not OrderStatus.StockConfirmed)
            {
                return;
            }

            var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(8));
            var token = cancellationTokenSource.Token;

            StartOrderPaymentTimerAsync(orderToUpdate, token);

            orderToUpdate.PropertyChanged += (sender, property) =>
            {
                if (property.PropertyName is not nameof(Order.Status))
                {
                    return;
                }

                RequestTimerCancellationOnNewOrderStatus(sender, cancellationTokenSource);
            };

        });

        hubConnectionService.OnOrderPaymentPageInitialized((orderId, approvalLink) =>
        {
            var orderToPay = Orders.FirstOrDefault(order => order.Id == orderId);
            if (orderToPay is null)
            {
                return;
            }

            orderToPay.PaymentApprovalLink = approvalLink;
        });
    }


    private static void RequestTimerCancellationOnNewOrderStatus(object sender, CancellationTokenSource cancellationTokenSource)
    {
        var updatedOrder = sender as Order;
        if (updatedOrder.Status is not OrderStatus.StockConfirmed)
        {
            cancellationTokenSource.Cancel();
        }
    }


    private static async void StartOrderPaymentTimerAsync(Order order, CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            order.TimeToProceedToPayment = TimeSpan.FromMinutes(8);

            do
            {
                order.TimeToProceedToPayment -= TimeSpan.FromSeconds(1);
                Thread.Sleep(TimeSpan.FromSeconds(1));

            } while (order.TimeToProceedToPayment != TimeSpan.Zero && !cancellationToken.IsCancellationRequested);

            order.PaymentApprovalLink = null;
        }, cancellationToken);
    }

    [RelayCommand]
    private async Task ProceedToPaymentAsync(Order order, CancellationToken cancellationToken)
    {
        await Shell.Current.GoToAsync(nameof(OrderPaymentView),
            new Dictionary<string, object> { { "Payment Approval Link", order.PaymentApprovalLink } });
    }
}