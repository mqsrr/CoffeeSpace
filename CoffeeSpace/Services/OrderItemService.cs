using System.Net.Http.Json;
using CoffeeSpace.Application;
using CoffeeSpace.Application.Contracts.Requests.OrderItem;
using CoffeeSpace.Application.Models.Orders;

namespace CoffeeSpace.Services;

public class OrderItemService : IOrderItemService
{
    private readonly HttpClient _client;
    
    public OrderItemService(HttpClient client)
    {
        _client = client;

        string baseAddress = DeviceInfo.Platform == DevicePlatform.Android
            ? "http://10.0.2.2/"
            : "https://localhost:7194/";

        _client.BaseAddress = new Uri(baseAddress);
    }

    public async Task<OrderItem?> GetByIdAsync(string id, CancellationToken token = default)
    {
        string formattedRequest = string.Format(ApiEndpoints.OrderItems.Get, id);

        OrderItem? response = await _client.GetFromJsonAsync<OrderItem>(formattedRequest, token);

        return response;

    }
    public async Task<IEnumerable<OrderItem>> GetAllAsync(CancellationToken token = default)
    {
        IEnumerable<OrderItem> orderItems = await _client.GetFromJsonAsync<IEnumerable<OrderItem>>( ApiEndpoints.OrderItems.GetAll, cancellationToken: token);

        return orderItems;
    }
    public Task<bool> CreateAsync(CreateOrderItemRequest request, CancellationToken token = default) => throw new NotImplementedException();

    public Task<OrderItem> UpdateAsync(UpdateOrderItemRequest request, CancellationToken token = default) => throw new NotImplementedException();
    public Task<bool> DeleteByIdAsync(string id, CancellationToken token = default) => throw new NotImplementedException();
}