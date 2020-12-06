using ProjectHermes.ShoppingList.Frontend.Infrastructure.Exceptions;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using WebAssembly;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection
{
    public class CommandQueue : ICommandQueue
    {
        private const int ConnectionRetryIntervalInMilliseconds = 4000;

        private readonly Timer timer;
        private readonly IApiClient commandClient;

        private bool connectionAlive = true;
        private readonly List<IApiRequest> queue = new List<IApiRequest>();

        private Func<Task> FirstRequestFailedCallback;
        private Func<Task> AllQueueItemsProcessedCallback;

        public CommandQueue(IApiClient commandClient)
        {
            timer = new Timer(ConnectionRetryIntervalInMilliseconds);
            timer.Elapsed += async (s, e) =>
            {
                if (!connectionAlive)
                    await RetryConnectionAsync();
            };
            timer.Start();
            this.commandClient = commandClient;
        }

        public void Initialize(Func<Task> firstRequestFailedCallback, Func<Task> allQueueItemsProcessedCallback)
        {
            FirstRequestFailedCallback = firstRequestFailedCallback;
            AllQueueItemsProcessedCallback = allQueueItemsProcessedCallback;
        }

        public async Task Enqueue(IApiRequest request)
        {
            lock (queue)
            {
                queue.Add(request);
            }

            if (connectionAlive && queue.Count == 1)
            {
                try
                {
                    await ProcessQueue();
                }
                catch (ApiConnectionException)
                {
                    await OnApiConnectionDied();
                }
            }
        }

        private async Task RetryConnectionAsync()
        {
            Console.WriteLine("Attempt connection retry.");
            try
            {
                await commandClient.IsAliveAsync();
            }
            catch (Exception)
            {
                Console.WriteLine("Connection still not available.");
                return;
            }

            Console.WriteLine("Established connection. Processing queue.");

            try
            {
                await ProcessQueue();
            }
            catch (ApiConnectionException)
            {
                await OnApiConnectionDied();
                return;
            }
            connectionAlive = true;

            await AllQueueItemsProcessedCallback.Invoke();
        }

        private async Task ProcessQueue()
        {
            while (true)
            {
                IApiRequest request;
                lock (queue)
                {
                    if (!queue.Any())
                    {
                        break;
                    }
                    request = queue.First();
                }

                try
                {
                    await SendRequest(request);
                }
                catch (System.Exception e)
                {
                    Console.WriteLine($"Encountered {e.GetType()} during request.");
                    throw new ApiConnectionException("", e);
                }

                lock (queue)
                {
                    queue.RemoveAt(0);
                }
            }
        }

        private async Task SendRequest(IApiRequest request)
        {
            switch (request.GetType().Name)
            {
                case nameof(PutItemInBasketRequest):
                    await commandClient.PutItemInBasketAsync((PutItemInBasketRequest)request);
                    break;
                case nameof(RemoveItemFromBasketRequest):
                    await commandClient.RemoveItemFromBasketAsync((RemoveItemFromBasketRequest)request);
                    break;
                case nameof(RemoveItemFromShoppingListRequest):
                    await commandClient.RemoveItemFromShoppingListAsync((RemoveItemFromShoppingListRequest)request);
                    break;
                case nameof(FinishListRequest):
                    await commandClient.FinishListAsync((FinishListRequest)request);
                    break;
                case nameof(ChangeItemQuantityOnShoppingListRequest):
                    await commandClient.ChangeItemQuantityOnShoppingListAsync(
                        (ChangeItemQuantityOnShoppingListRequest)request);
                    break;
                case nameof(CreateTemporaryItemRequest):
                    await commandClient.CreateTemporaryItem(
                        (CreateTemporaryItemRequest)request);
                    break;
                case nameof(AddItemToShoppingListRequest):
                    await commandClient.AddItemToShoppingListAsync(
                        (AddItemToShoppingListRequest)request);
                    break;
            }
        }

        private async Task OnApiConnectionDied()
        {
            connectionAlive = false;
            await FirstRequestFailedCallback.Invoke();
        }
    }
}