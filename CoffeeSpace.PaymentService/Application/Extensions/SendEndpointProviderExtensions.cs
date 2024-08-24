using MassTransit;

namespace CoffeeSpace.PaymentService.Application.Extensions;

internal static class SendEndpointProviderExtensions
{
    internal static async Task Send<TMessage>(
        this ISendEndpointProvider provider,
        Uri endpointUri,
        object values,
        Action<SendContext<TMessage>> context,
        CancellationToken cancellationToken)
        where TMessage : class
    {
        var sendEndpoint = await provider.GetSendEndpoint(endpointUri);
        await sendEndpoint.Send(values, context, cancellationToken).ConfigureAwait(false);
    }
}