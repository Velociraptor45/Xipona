using ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection;
using ShoppingList.Frontend.Redux.Shared.Ports;
using ShoppingList.Frontend.Redux.Shared.Ports.Requests;
using System;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.RequestSenders;

public interface IRequestSender
{
    Type RequestType { get; }

    Task SendAsync(IApiClient client, IApiRequest request);
}