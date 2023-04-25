#nullable enable
using System.Net.Http.Json;
using CoffeeSpace.Application.Contracts.Requests.Customer;
using CoffeeSpace.Application.Models.CustomerInfo;

namespace CoffeeSpace.Services;

public sealed class CustomerService : ICustomerService
{
    private readonly HttpClient _client;
    
    public CustomerService(HttpClient client)
    {
        _client = client;

        string baseAddress = DeviceInfo.Platform == DevicePlatform.Android
            ? "http://10.0.2.2/"
            : "https://localhost:7194/";

        _client.BaseAddress = new Uri(baseAddress);
    }

    public async Task<Customer?> GetByIdAsync(string id, CancellationToken token = default)
    {
        string formattedRequest = string.Format(_client.BaseAddress + "api/customers/{0}", id);

        Customer? response = await _client.GetFromJsonAsync<Customer>(formattedRequest, token);

        return response;

    }
    public Task<Customer> GetByEmailAsync(string email, CancellationToken token = default) => throw new NotImplementedException();
    public Task<IEnumerable<Customer>> GetAllAsync(CancellationToken token = default) => throw new NotImplementedException();
    public Task<bool> CreateAsync(CreateCustomerRequest request, CancellationToken token = default) => throw new NotImplementedException();

    public Task<Customer?> UpdateAsync(UpdateCustomerRequest request, CancellationToken token = default) => throw new NotImplementedException();
    public Task<bool> DeleteByIdAsync(string id, CancellationToken token = default) => throw new NotImplementedException();
}