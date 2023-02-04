using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports
{
    public interface ICommandQueue
    {
        Task Enqueue(IApiRequest request);

        //void Initialize(ICommandQueueErrorHandler errorHandler);
    }
}