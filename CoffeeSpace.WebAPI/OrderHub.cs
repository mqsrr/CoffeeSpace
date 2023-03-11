using CoffeeSpace.Application.Models.Orders;
using Microsoft.AspNetCore.SignalR;

namespace CoffeeSpace.WebAPI;

public class OrderHub : Hub
{
    public Task SendOrder(Order order) => Clients.All.SendAsync("OrderReceived", order);

}