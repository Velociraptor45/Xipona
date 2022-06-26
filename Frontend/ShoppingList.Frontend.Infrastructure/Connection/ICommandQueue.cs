using ProjectHermes.ShoppingList.Frontend.Infrastructure.Error;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection
{
    public interface ICommandQueue
    {
        Task Enqueue(IApiRequest request);

        void Initialize(ICommandQueueErrorHandler errorHandler);
    }
}