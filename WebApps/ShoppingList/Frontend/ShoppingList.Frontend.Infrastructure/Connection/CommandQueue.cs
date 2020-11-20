using Microsoft.AspNetCore.Components;
using ShoppingList.Frontend.Models.Shared.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using WebAssembly;

namespace ShoppingList.Frontend.Infrastructure.Connection
{
    public class CommandQueue : ICommandQueue
    {
        private const int ConnectionRetryIntervalInMilliseconds = 2000;

        private readonly Timer timer;
        private readonly ICommandClient commandClient;

        private bool connectionAlive = true;
        private List<IApiRequest> queue = new List<IApiRequest>();

        private EventCallback FirstRequestFailedCallback;
        private EventCallback AllQueueItemsProcessedCallback;

        public CommandQueue(ICommandClient commandClient)
        {
            timer = new Timer(ConnectionRetryIntervalInMilliseconds);
            timer.Elapsed += async (s, e) =>
            {
                if (!connectionAlive)
                    await RetryConnectionAsync();
            };
            this.commandClient = commandClient;
        }

        public void Initialize(EventCallback firstRequestFailedCallback, EventCallback allQueueItemsProcessedCallback)
        {
            FirstRequestFailedCallback = firstRequestFailedCallback;
            AllQueueItemsProcessedCallback = allQueueItemsProcessedCallback;
        }

        public void Enqueue(IApiRequest request)
        {
            queue.Add(request);
        }

        private Task RetryConnectionAsync()
        {
            try
            {
                //todo call alive endpoint
            }
            catch (JSException e)
            {
                return Task.CompletedTask;
            }

            connectionAlive = true;
            return Task.CompletedTask;
        }

        private async Task SendRequest(IApiRequest request)
        {
            switch (request.GetType().Name)
            {
                case nameof(PutItemInBasketRequest):
                    await commandClient.PutItemInBasketAsync((PutItemInBasketRequest)request);
                    break;
            }
        }
    }
}