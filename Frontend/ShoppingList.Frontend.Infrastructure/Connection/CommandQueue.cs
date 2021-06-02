using ProjectHermes.ShoppingList.Frontend.Infrastructure.Exceptions;
using ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests;
using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Timers;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection
{
    public class CommandQueue : ICommandQueue
    {
        private const int ConnectionRetryIntervalInMilliseconds = 4000;

        private readonly Timer timer;
        private readonly IApiClient commandClient;

        private bool connectionAlive = true;
        private readonly List<IApiRequest> queue = new();

        private Func<Task> FirstRequestFailedCallback;
        private Func<Task> AllQueueItemsProcessedCallback;
        private Action ApiProcessingErrorCallback;
        private Action<string> DebugCallback;

        public CommandQueue(IApiClient commandClient)
        {
            timer = new Timer(ConnectionRetryIntervalInMilliseconds);
            this.commandClient = commandClient;
        }

        public void Initialize(
            Func<Task> firstRequestFailedCallback,
            Func<Task> allQueueItemsProcessedCallback,
            Action apiProcessingErrorCallback,
            Action<string> debugCallback)
        {
            FirstRequestFailedCallback = firstRequestFailedCallback;
            AllQueueItemsProcessedCallback = allQueueItemsProcessedCallback;
            ApiProcessingErrorCallback = apiProcessingErrorCallback;
            DebugCallback = debugCallback;

            try
            {
                timer.Elapsed += async (s, e) =>
                {
                    if (!connectionAlive)
                        await RetryConnectionAsync();
                };
                timer.Start();
            }
            catch (Exception e)
            {
                DebugCallback.Invoke(e.ToString());
            }
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
                catch (ApiProcessingException e)
                {
                    OnApiProcessingError();
                    DebugCallback.Invoke(e.InnerException.ToString());
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
            catch (ApiProcessingException e)
            {
                OnApiProcessingError();
                DebugCallback.Invoke(e.InnerException.ToString());
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
                catch (ApiException e)
                {
                    Console.WriteLine($"Encountered {e.GetType()} during request.");

                    if (e.StatusCode == HttpStatusCode.BadRequest
                        || e.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        throw new ApiProcessingException("An error occured while processing the request. See inner exception for more details.", e);
                    }

                    throw new ApiConnectionException("An api error occured while processing the request. See inner exception for more details.", e);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Encountered {e.GetType()} during request.");
                    throw new ApiConnectionException("An unknown error occured. See inner exception for more details.", e);
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

        private void OnApiProcessingError()
        {
            lock (queue)
            {
                queue.Clear();
            }
            ApiProcessingErrorCallback.Invoke();
        }

        private async Task OnApiConnectionDied()
        {
            connectionAlive = false;
            await FirstRequestFailedCallback.Invoke();
        }
    }
}