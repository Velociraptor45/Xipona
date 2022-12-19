using ShoppingList.Frontend.Redux.Shared.Ports.Requests;

namespace ShoppingList.Frontend.Redux.Shared.Ports
{
    public interface ICommandQueue
    {
        Task Enqueue(IApiRequest request);

        //void Initialize(ICommandQueueErrorHandler errorHandler);
    }
}