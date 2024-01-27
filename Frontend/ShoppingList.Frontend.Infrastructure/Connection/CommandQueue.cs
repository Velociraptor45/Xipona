using Fluxor;
using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Exceptions;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.RequestSenders;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions.Processing;
using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection;

public class CommandQueue : ICommandQueue, IDisposable
{
    private readonly IApiClient _commandClient;
    private readonly IRequestSenderStrategy _senderStrategy;
    private readonly IDispatcher _dispatcher;
    private readonly ILogger<CommandQueue> _logger;

    private bool _connectionAlive = true;
    private static readonly List<IApiRequest> _queue = new();
    private readonly Timer _timer;

    public CommandQueue(IApiClient commandClient, IRequestSenderStrategy senderStrategy, IDispatcher dispatcher,
        CommandQueueConfig config, ILogger<CommandQueue> logger)
    {
        _commandClient = commandClient;
        _senderStrategy = senderStrategy;
        _dispatcher = dispatcher;
        _logger = logger;

        if (config.ConnectionRetryInterval == TimeSpan.Zero)
            return;

        try
        {
            _timer = new Timer(config.ConnectionRetryInterval);
            _timer.Elapsed += async (_, _) =>
            {
                if (!_connectionAlive)
                    await RetryConnectionAsync();
            };
            _timer.Start();
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
            await SafeProcessQueue();
        }
    }

    internal async Task RetryConnectionAsync()
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

        if (!await SafeProcessQueue())
            return;

        _connectionAlive = true;

        _dispatcher.Dispatch(new ApiConnectionRecoveredAction());
        _dispatcher.Dispatch(new QueueProcessedAction());
    }

    private async Task<bool> SafeProcessQueue()
    {
        var report = new ProcessingReport();
        try
        {
            await ProcessQueue(report);
            return true;
        }
        catch (ApiConnectionException)
        {
            OnApiConnectionDied();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An unexpected Exception occurred. Clearing the queue");
            _dispatcher.Dispatch(new LogAction(e.ToString()));
            lock (_queue)
            {
                _queue.Clear();
            }
        }
        finally
        {
            if (report.NeedsReload)
                _dispatcher.Dispatch(new ReloadCurrentShoppingListAction());
        }

        return false;
    }

    private async Task ProcessQueue(ProcessingReport report)
    {
        while (true)
        {
            IApiRequest request;
            lock (_queue)
            {
                if (_queue.Count == 0)
                    break;

                request = _queue.First();
            }

            try
            {
                await SendRequest(request);
            }
            catch (ApiException e)
            {
                _logger.LogError(e, "Api exception occurred with Message: {Message}", e.Content);
                HandleRequestFailure(report, request, e, e.StatusCode);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, "HttpRequestException during request");
                HandleRequestFailure(report, request, e, e.StatusCode);
            }

            lock (_queue)
            {
                _queue.Remove(request);
            }
        }
    }

    private void HandleRequestFailure(ProcessingReport report, IApiRequest failedRequest, Exception e, HttpStatusCode? statusCode)
    {
        Console.WriteLine($"Encountered {e.GetType()} during request.");

        if (statusCode is HttpStatusCode.BadRequest
            or HttpStatusCode.InternalServerError
            or HttpStatusCode.UnprocessableEntity)
        {
            _dispatcher.Dispatch(new ApiRequestProcessingErrorOccurredAction(failedRequest));
            _dispatcher.Dispatch(new LogAction(e.ToString()));
            report.RequestReload();
            return;
        }

        throw new ApiConnectionException("An api error occurred while processing the request. See inner exception for more details.", e);
    }

    private async Task SendRequest(IApiRequest request)
    {
        await _senderStrategy.SendAsync(request);
    }

    private void OnApiConnectionDied()
    {
        _connectionAlive = false;
        _dispatcher.Dispatch(new ApiConnectionDiedAction());
    }

    private sealed class ProcessingReport
    {
        public bool NeedsReload { get; private set; }

        public void RequestReload()
        {
            NeedsReload = true;
        }
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}