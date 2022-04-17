using ProjectHermes.ShoppingList.Frontend.Infrastructure.Error;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Exceptions;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.Items;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.ShoppingLists;
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
        private const int _connectionRetryIntervalInMilliseconds = 4000;

        private readonly Timer _timer;
        private readonly IApiClient _commandClient;

        private bool _connectionAlive = true;
        private readonly List<IApiRequest> _queue = new();

        private ICommandQueueErrorHandler _errorHandler;

        public CommandQueue(IApiClient commandClient)
        {
            _timer = new Timer(_connectionRetryIntervalInMilliseconds);
            _commandClient = commandClient;
        }

        public void Initialize(ICommandQueueErrorHandler errorHandler)
        {
            _errorHandler = errorHandler;

            try
            {
                _timer.Elapsed += async (_, _) =>
                {
                    if (!_connectionAlive)
                        await RetryConnectionAsync();
                };
                _timer.Start();
            }
            catch (Exception e)
            {
                errorHandler.Log(e.ToString());
            }
        }

        public async Task Enqueue(IApiRequest request)
        {
            lock (_queue)
            {
                _queue.Add(request);
            }

            if (_connectionAlive && _queue.Count == 1)
            {
                try
                {
                    await ProcessQueue();
                }
                catch (ApiConnectionException)
                {
                    OnApiConnectionDied();
                }
                catch (ApiProcessingException e)
                {
                    OnApiProcessingError();
                    _errorHandler.Log(e.InnerException.ToString());
                }
            }
        }

        private async Task RetryConnectionAsync()
        {
            Console.WriteLine("Attempt connection retry.");
            try
            {
                await _commandClient.IsAliveAsync();
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
                OnApiConnectionDied();
                return;
            }
            catch (ApiProcessingException e)
            {
                OnApiProcessingError();
                _errorHandler.Log(e.InnerException.ToString());
                return;
            }
            _connectionAlive = true;

            await _errorHandler.OnQueueProcessedAsync();
        }

        private async Task ProcessQueue()
        {
            while (true)
            {
                IApiRequest request;
                lock (_queue)
                {
                    if (!_queue.Any())
                    {
                        break;
                    }
                    request = _queue.First();
                }

                try
                {
                    await SendRequest(request);
                }
                catch (ApiException e)
                {
                    Console.WriteLine($"Encountered {e.GetType()} during request.");

                    if (e.StatusCode is HttpStatusCode.BadRequest
                        or HttpStatusCode.InternalServerError
                        or HttpStatusCode.UnprocessableEntity)
                    {
                        throw new ApiProcessingException("An error occurred while processing the request. See inner exception for more details.", e);
                    }

                    throw new ApiConnectionException("An api error occurred while processing the request. See inner exception for more details.", e);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Encountered {e.GetType()} during request.");
                    throw new ApiConnectionException("An unknown error occurred. See inner exception for more details.", e);
                }

                lock (_queue)
                {
                    _queue.RemoveAt(0);
                }
            }
        }

        private async Task SendRequest(IApiRequest request)
        {
            switch (request.GetType().Name)
            {
                case nameof(PutItemInBasketRequest):
                    await _commandClient.PutItemInBasketAsync((PutItemInBasketRequest)request);
                    break;
                case nameof(RemoveItemFromBasketRequest):
                    await _commandClient.RemoveItemFromBasketAsync((RemoveItemFromBasketRequest)request);
                    break;
                case nameof(RemoveItemFromShoppingListRequest):
                    await _commandClient.RemoveItemFromShoppingListAsync((RemoveItemFromShoppingListRequest)request);
                    break;
                case nameof(FinishListRequest):
                    await _commandClient.FinishListAsync((FinishListRequest)request);
                    break;
                case nameof(ChangeItemQuantityOnShoppingListRequest):
                    await _commandClient.ChangeItemQuantityOnShoppingListAsync(
                        (ChangeItemQuantityOnShoppingListRequest)request);
                    break;
                case nameof(CreateTemporaryItemRequest):
                    await _commandClient.CreateTemporaryItem(
                        (CreateTemporaryItemRequest)request);
                    break;
                case nameof(AddItemToShoppingListRequest):
                    await _commandClient.AddItemToShoppingListAsync(
                        (AddItemToShoppingListRequest)request);
                    break;
            }
        }

        private void OnApiProcessingError()
        {
            lock (_queue)
            {
                _queue.Clear();
            }
            _errorHandler.OnApiProcessingError();
        }

        private void OnApiConnectionDied()
        {
            _connectionAlive = false;
            _errorHandler.OnConnectionFailed();
        }
    }
}