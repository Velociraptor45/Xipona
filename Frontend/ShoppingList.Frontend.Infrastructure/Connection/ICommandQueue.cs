using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;
using System;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection
{
    public interface ICommandQueue
    {
        Task Enqueue(IApiRequest request);

        void Initialize(Func<Task> firstRequestFailedCallback, Func<Task> allQueueItemsProcessedCallback);
    }
}