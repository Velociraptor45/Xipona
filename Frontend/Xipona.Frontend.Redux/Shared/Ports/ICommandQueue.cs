using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests;

namespace ProjectHermes.Xipona.Frontend.Redux.Shared.Ports
{
    public interface ICommandQueue
    {
        Task Enqueue(IApiRequest request);
    }
}