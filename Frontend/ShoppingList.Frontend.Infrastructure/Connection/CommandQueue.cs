using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Exceptions;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.RequestSenders;
using RestEase;
using ShoppingList.Frontend.Redux.Shared.Ports;
using ShoppingList.Frontend.Redux.Shared.Ports.Requests;
using ShoppingList.Frontend.Redux.ShoppingList.Actions.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection
{
    public class CommandQueue : ICommandQueue
    {
        private const int _connectionRetryIntervalInMilliseconds = 4000;

        private readonly IApiClient _commandClient;
        private readonly IRequestSenderStrategy _senderStrategy;
        private readonly IDispatcher _dispatcher;

        private bool _connectionAlive = true;
        private readonly List<IApiRequest> _queue = new();

        public CommandQueue(IApiClient commandClient, IRequestSenderStrategy senderStrategy, IDispatcher dispatcher)
        {
            _commandClient = commandClient;
            _senderStrategy = senderStrategy;
            _dispatcher = dispatcher;

            try
            {
                var timer = new Timer(_connectionRetryIntervalInMilliseconds);
                timer.Elapsed += async (_, _) =>
                {
                    if (!_connectionAlive)
                        await RetryConnectionAsync();
                };
                timer.Start();
            }
            catch (Exception e)
            {
                _dispatcher.Dispatch(new LogAction(e.ToString()));
            }
        }

        public async Task Enqueue(IApiRequest request)
        {
            int queueCount;
            lock (_queue)
            {
                _queue.Add(request);
                queueCount = _queue.Count;
            }

            if (_connectionAlive && queueCount == 1)
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
                    _dispatcher.Dispatch(new LogAction(e.InnerException.ToString()));
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
                _dispatcher.Dispatch(new LogAction(e.InnerException.ToString()));
                return;
            }
            _connectionAlive = true;

            _dispatcher.Dispatch(new QueueProcessedAction());
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
                    HandleRequestFailure(e, e.StatusCode);
                }
                catch (HttpRequestException e)
                {
                    HandleRequestFailure(e, e.StatusCode);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Encountered {e.GetType()} during request.");
                    throw;
                }

                lock (_queue)
                {
                    _queue.RemoveAt(0);
                }
            }
        }

        private static void HandleRequestFailure(Exception e, HttpStatusCode? statusCode)
        {
            Console.WriteLine($"Encountered {e.GetType()} during request.");

            if (statusCode is HttpStatusCode.BadRequest
                or HttpStatusCode.InternalServerError
                or HttpStatusCode.UnprocessableEntity)
            {
                throw new ApiProcessingException("An error occurred while processing the request. See inner exception for more details.", e);
            }

            throw new ApiConnectionException("An api error occurred while processing the request. See inner exception for more details.", e);
        }

        private async Task SendRequest(IApiRequest request)
        {
            await _senderStrategy.SendAsync(request);
        }

        private void OnApiProcessingError()
        {
            lock (_queue)
            {
                _queue.Clear();
            }
            _dispatcher.Dispatch(new ApiRequestProcessingErrorOccurredAction());
        }

        private void OnApiConnectionDied()
        {
            _connectionAlive = false;
            _dispatcher.Dispatch(new ApiConnectionDiedAction());
        }
    }
}