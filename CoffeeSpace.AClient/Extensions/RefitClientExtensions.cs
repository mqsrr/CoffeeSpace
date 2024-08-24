using System;
using System.Net.Http;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CoffeeSpace.AClient.Helpers;
using CoffeeSpace.AClient.HttpHandlers;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace CoffeeSpace.AClient.Extensions;

public static class RefitClientExtensions
{
    public static IServiceCollection AddWebApiClient<TClient, TMessageHandler>(this IServiceCollection services)
        where TClient : class
        where TMessageHandler : DelegatingHandler
    {
        string baseAddress = Application.Current!.ApplicationLifetime is ISingleViewApplicationLifetime
            ? ApiEndpoints.BasePhoneAddress
            : ApiEndpoints.BaseAddress;

        services.AddRefitClient<TClient>()
            .ConfigureHttpClient(config => config.BaseAddress = new Uri(baseAddress))
            .AddHttpMessageHandler<TMessageHandler>();

        return services;
    }
    
    public static IServiceCollection AddWebApiClient<TClient>(this IServiceCollection services)
        where TClient : class
    {
        string baseAddress = Application.Current!.ApplicationLifetime is ISingleViewApplicationLifetime
            ? ApiEndpoints.BasePhoneAddress
            : ApiEndpoints.BaseAddress;

        services.AddRefitClient<TClient>()
            .ConfigureHttpClient(config => config.BaseAddress = new Uri(baseAddress))
            .AddHttpMessageHandler<BearerAuthorizationMessageHandler>();

        return services;
    }
}