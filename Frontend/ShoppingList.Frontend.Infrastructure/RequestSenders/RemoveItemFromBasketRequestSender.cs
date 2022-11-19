using ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.ShoppingLists;
using System;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.RequestSenders;

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