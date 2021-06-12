using ProjectHermes.ShoppingList.Frontend.Models.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection
{
    public interface ICommandQueue
    {
        Task Enqueue(IApiRequest request);

        void Initialize(ICommandQueueErrorHandler errorHandler);
    }
}