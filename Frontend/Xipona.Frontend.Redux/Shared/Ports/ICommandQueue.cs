using Xipona.Frontend.Redux.Shared.Ports.Requests;

namespace Xipona.Frontend.Redux.Shared.Ports
{
    public interface ICommandQueue
    {
        Task Enqueue(IApiRequest request);
    }
}