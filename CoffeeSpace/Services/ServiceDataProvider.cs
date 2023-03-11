using System.Net.Http.Json;
using CoffeeSpace.Application.Models.Orders;

namespace CoffeeSpace.Services;

public sealed class ServiceDataProvider<TEntity> : IServiceDataProvider<TEntity>
{
    private readonly HttpClient _client;
    
    public ServiceDataProvider(HttpClient client)
    {
        _client = client;

        string basePath = DeviceInfo.Platform == DevicePlatform.Android
            ? $"http://10.0.2.2:5109/api/{typeof(TEntity).Name}s"
            : $"https://localhost:7194/api/{typeof(TEntity).Name}s";

        _client.BaseAddress = new Uri(basePath);
    }
    
    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token = default)
    {
        HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress, token);

        IEnumerable<TEntity> entities = await response.Content.ReadFromJsonAsync<IEnumerable<TEntity>>(cancellationToken: token);
        
        ArgumentNullException.ThrowIfNull(entities);

        return entities;
    }

    public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        string formattedPath = string.Format(_client.BaseAddress + "/{0}", id);
        
        HttpResponseMessage response = await _client.GetAsync(formattedPath, token);

        TEntity entity = await response.Content.ReadFromJsonAsync<TEntity>(cancellationToken: token);
        
        ArgumentNullException.ThrowIfNull(entity);

        return entity;
    }

    public async Task AddAsync(TEntity entity, CancellationToken token = default)
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync(_client.BaseAddress, entity,token);

        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken token = default)
    {
        HttpResponseMessage response = await _client.PutAsJsonAsync(_client.BaseAddress, entity, token);

        response.EnsureSuccessStatusCode();
    }

    public async Task Delete(Guid id, CancellationToken token = default)
    {
        string formattedPath = string.Format(_client.BaseAddress + "/{0}", id);
        
        await _client.DeleteFromJsonAsync<TEntity>(formattedPath, token);
    }
}