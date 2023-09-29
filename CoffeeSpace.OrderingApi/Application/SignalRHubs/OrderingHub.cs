using CoffeeSpace.OrderingApi.Application.SignalRHubs.Abstraction;
using Microsoft.AspNetCore.SignalR;

namespace CoffeeSpace.OrderingApi.Application.SignalRHubs;

internal sealed class OrderingHub : Hub<IOrderingHub>
{
    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }
    
    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
}