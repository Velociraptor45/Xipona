using ProjectHermes.Xipona.Frontend.Infrastructure.Connection;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using System;
using System.Threading.Tasks;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.RequestSenders;

public class RemoveItemFromShoppingListRequestSender : IRequestSender
{
    public Type RequestType => typeof(RemoveItemFromShoppingListRequest);

    public async Task SendAsync(IApiClient client, IApiRequest request)
    {
        if (request.GetType() != RequestType)
            throw new ArgumentException($"Request is not type of {RequestType.Name}", nameof(request));

        await client.RemoveItemFromShoppingListAsync((RemoveItemFromShoppingListRequest)request);
    }
}