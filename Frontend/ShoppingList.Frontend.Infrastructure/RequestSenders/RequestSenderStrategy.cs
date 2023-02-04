using ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.RequestSenders;

public class RequestSenderStrategy : IRequestSenderStrategy
{
    private readonly IReadOnlyCollection<IRequestSender> _requestSenders;
    private readonly IApiClient _client;

    public RequestSenderStrategy(IEnumerable<IRequestSender> requestSenders, IApiClient client)
    {
        _requestSenders = requestSenders.ToArray();
        _client = client;
    }

    public async Task SendAsync(IApiRequest request)
    {
        var sender = _requestSenders.FirstOrDefault(s => s.RequestType == request.GetType());
        if (sender is null)
            throw new NotSupportedException($"No sender for request type {request.GetType().Name} registered");

        await sender.SendAsync(_client, request);
    }
}