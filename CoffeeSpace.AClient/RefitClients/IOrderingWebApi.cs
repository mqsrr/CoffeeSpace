using System;
using System.Threading;
using System.Threading.Tasks;
using CoffeeSpace.AClient.Contracts.Requests.Orders;
using CoffeeSpace.AClient.Helpers;
using CoffeeSpace.AClient.Models;
using Refit;

namespace CoffeeSpace.AClient.RefitClients;

[Headers("Bearer")]
public interface IOrderingWebApi
{
    [Post(ApiEndpoints.Orders.Create)]
    Task<IApiResponse<Order>> CreateOrder(Guid buyerId, [Body]CreateOrderRequest order, CancellationToken cancellationToken);
}