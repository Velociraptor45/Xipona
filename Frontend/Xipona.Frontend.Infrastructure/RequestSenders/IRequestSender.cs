using Xipona.Frontend.Infrastructure.Connection;
using Xipona.Frontend.Redux.Shared.Ports;
using Xipona.Frontend.Redux.Shared.Ports.Requests;
using System;
using System.Threading.Tasks;

namespace Xipona.Frontend.Infrastructure.RequestSenders;

public interface IRequestSender
{
    Type RequestType { get; }

    Task SendAsync(IApiClient client, IApiRequest request);
}