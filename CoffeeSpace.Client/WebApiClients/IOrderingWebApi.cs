using CoffeeSpace.Client.Helpers;
using CoffeeSpace.Client.Models.Ordering;
using Refit;

namespace CoffeeSpace.Client.WebApiClients;

[Headers("Authorization: Bearer")]
public interface IOrderingWebApi
{
    [Get(ApiEndpoints.Orders.GetAll)]
    Task<IEnumerable<Order>> GetAllOrdersByBuyerId(Guid buyerId, CancellationToken cancellationToken);
    
    [Get(ApiEndpoints.Orders.Get)]
    Task<Order> GetOrderByBuyerId(Guid buyerId, Guid id, CancellationToken cancellationToken);
    
    [Get(ApiEndpoints.Orders.Create)]
    Task<HttpResponseMessage> CreateOrder(Guid buyerId, CancellationToken cancellationToken);
    
    [Get(ApiEndpoints.Orders.Delete)]
    Task<HttpResponseMessage> DeleteOrder(Guid buyerId, Guid id, CancellationToken cancellationToken);
    
}