using Xipona.Frontend.Redux.Shared.Ports.Requests;
using System.Threading.Tasks;

namespace Xipona.Frontend.Infrastructure.RequestSenders;

public interface IRequestSenderStrategy
{
    Task SendAsync(IApiRequest request);
}