using ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection;
using ShoppingList.Frontend.Redux.Shared.Ports;
using ShoppingList.Frontend.Redux.Shared.Ports.Requests;
using ShoppingList.Frontend.Redux.Shared.Ports.Requests.Items;
using System;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.RequestSenders;

public class CreateTemporaryItemRequestSender : IRequestSender
{
    public Type RequestType => typeof(CreateTemporaryItemRequest);

    public async Task SendAsync(IApiClient client, IApiRequest request)
    {
        if (request.GetType() != RequestType)
            throw new ArgumentException($"Request is not type of {RequestType.Name}", nameof(request));

        await client.CreateTemporaryItem((CreateTemporaryItemRequest)request);
    }
}