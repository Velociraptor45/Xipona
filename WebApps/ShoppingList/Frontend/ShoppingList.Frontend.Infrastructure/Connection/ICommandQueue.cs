using ShoppingList.Frontend.Models.Shared.Requests;

namespace ShoppingList.Frontend.Infrastructure.Connection
{
    public interface ICommandQueue
    {
        void Enqueue(IApiRequest request);
    }
}