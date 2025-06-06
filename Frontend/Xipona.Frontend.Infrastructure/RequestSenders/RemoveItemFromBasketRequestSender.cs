using Xipona.Frontend.Infrastructure.Connection;
using Xipona.Frontend.Redux.Shared.Ports;
using Xipona.Frontend.Redux.Shared.Ports.Requests;
using Xipona.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using System;
using System.Threading.Tasks;

namespace Xipona.Frontend.Infrastructure.RequestSenders;

public class RemoveItemFromBasketRequestSender : IRequestSender
{
    public Type RequestType => typeof(RemoveItemFromBasketRequest);

    public async Task SendAsync(IApiClient client, IApiRequest request)
    {
        if (request.GetType() != RequestType)
            throw new ArgumentException($"Request is not type of {RequestType.Name}", nameof(request));

        await client.RemoveItemFromBasketAsync((RemoveItemFromBasketRequest)request);
    }
}