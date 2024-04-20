using ProjectHermes.Xipona.Frontend.Infrastructure.Connection;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests;
using System;
using System.Threading.Tasks;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.RequestSenders;

public interface IRequestSender
{
    Type RequestType { get; }

    Task SendAsync(IApiClient client, IApiRequest request);
}