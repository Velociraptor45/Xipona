using Fluxor;
using Xipona.Frontend.Redux.Shared.Ports;
using Xipona.Frontend.Redux.Shared.Ports.Requests;
using Xipona.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using Xipona.Frontend.Redux.ShoppingList.Actions.TemporaryItemCreator;
using System;
using System.Threading.Tasks;

namespace Xipona.Frontend.Infrastructure.RequestSenders;

public class AddTemporaryItemToShoppingListRequestSender : IRequestSender
{
    private readonly IDispatcher _dispatcher;

    public AddTemporaryItemToShoppingListRequestSender(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public Type RequestType => typeof(AddTemporaryItemToShoppingListRequest);

    public async Task SendAsync(IApiClient client, IApiRequest request)
    {
        if (request.GetType() != RequestType)
            throw new ArgumentException($"Request is not type of {RequestType.Name}", nameof(request));

        var addRequest = (AddTemporaryItemToShoppingListRequest)request;
        var tempItem = await client.AddTemporaryItemToShoppingListAsync(addRequest);

        _dispatcher.Dispatch(new TemporaryItemCreatedAction(addRequest.TemporaryId, tempItem.ItemId));
    }
}