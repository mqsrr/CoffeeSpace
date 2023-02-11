using CoffeeSpace.Data.Models.Orders;
using Microsoft.AspNetCore.SignalR;

namespace CoffeeSpace.Serveless;

public class OrderHub : Hub
{
    public Task SendOrder(Order order) => Clients.All.SendAsync("OrderReceived", order);

}