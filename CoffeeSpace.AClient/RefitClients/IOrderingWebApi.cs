using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CoffeeSpace.AClient.Contracts.Requests.Orders;
using CoffeeSpace.AClient.Helpers;
using CoffeeSpace.AClient.Models;
using Refit;

namespace CoffeeSpace.AClient.RefitClients;

[Headers("Authorization: Bearer")]
public interface IOrderingWebApi
{
    [Get(ApiEndpoints.Orders.GetAll)]
    Task<IEnumerable<Order>> GetAllOrdersByBuyerId(Guid buyerId, CancellationToken cancellationToken);
    
    [Get(ApiEndpoints.Orders.Get)]
    Task<Order> GetOrderByBuyerId(Guid buyerId, Guid id, CancellationToken cancellationToken);
    
    [Post(ApiEndpoints.Orders.Create)]
    Task<Order> CreateOrder(Guid buyerId, [Body]CreateOrderRequest order, CancellationToken cancellationToken);
    
    [Delete(ApiEndpoints.Orders.Delete)]
    Task<HttpResponseMessage> DeleteOrder(Guid buyerId, Guid id, CancellationToken cancellationToken);
}