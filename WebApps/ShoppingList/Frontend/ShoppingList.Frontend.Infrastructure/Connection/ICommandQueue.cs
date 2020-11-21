using ShoppingList.Frontend.Models.Shared.Requests;
using System;
using System.Threading.Tasks;

namespace ShoppingList.Frontend.Infrastructure.Connection
{
    public interface ICommandQueue
    {
        Task Enqueue(IApiRequest request);

        void Initialize(Action firstRequestFailedCallback, Action allQueueItemsProcessedCallback);
    }
}