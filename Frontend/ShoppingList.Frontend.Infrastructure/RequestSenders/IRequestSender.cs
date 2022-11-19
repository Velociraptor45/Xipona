using ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests;
using System;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.RequestSenders;

public interface IRequestSender
{
    Type RequestType { get; }

    Task SendAsync(IApiClient client, IApiRequest request);
}