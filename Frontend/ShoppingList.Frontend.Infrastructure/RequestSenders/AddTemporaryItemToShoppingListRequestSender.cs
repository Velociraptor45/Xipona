using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using System;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.RequestSenders;

public class AddTemporaryItemToShoppingListRequestSender : IRequestSender
{
    public Type RequestType => typeof(AddTemporaryItemToShoppingListRequest);

    public async Task SendAsync(IApiClient client, IApiRequest request)
    {
        if (request.GetType() != RequestType)
            throw new ArgumentException($"Request is not type of {RequestType.Name}", nameof(request));

        await client.AddTemporaryItemToShoppingListAsync((AddTemporaryItemToShoppingListRequest)request);
    }
}