using Microsoft.AspNetCore.Components;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Error;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Error;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Notification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Index.Models
{
    public class ErrorHandler : ICommandQueueErrorHandler, IDebugHandler, IRetryFragmentCreator, IAsyncRetryFragmentCreator
    {
        private readonly List<string> _stack = new List<string>();
        private readonly Func<Action, string, RenderFragment> _createRenderFragment;
        private readonly Func<Func<Task>, string, RenderFragment> _createAsyncRenderFragment;
        private readonly IShoppingListNotificationService _notificationService;

        public Action StateChanged { get; set; }
        public Func<Task> QueueProcessed { get; set; }
        public IReadOnlyCollection<string> Stack => _stack.AsReadOnly();
        public bool IsDebug { get; private set; }
        public bool ApiHasProcessingError { get; private set; }

        public ErrorHandler(bool isDebug,
            Func<Action, string, RenderFragment> createRenderFragment,
            Func<Func<Task>, string, RenderFragment> createAsyncRenderFragment,
            IShoppingListNotificationService notificationService)
        {
            IsDebug = isDebug;
            _createRenderFragment = createRenderFragment;
            _createAsyncRenderFragment = createAsyncRenderFragment;
            _notificationService = notificationService;
        }

        public void ToggleDebugState()
        {
            IsDebug = !IsDebug;
        }

        public RenderFragment CreateRetryFragment(Action action)
        {
            return _createRenderFragment(action, "Retry");
        }

        public RenderFragment CreateAsyncRetryFragment(Func<Task> func)
        {
            return _createAsyncRenderFragment(func, "Retry");
        }

        public void Log(string content)
        {
            if (!IsDebug)
                return;

            _stack.Add(content);
            Console.WriteLine(content);
            StateChanged();
        }

        public void OnConnectionFailed()
        {
            Log("Connection failed");
            _notificationService.NotifyWarning("Connection interrupted", "Connection to the server was interrupted.");
        }

        public async Task OnQueueProcessedAsync()
        {
            Log("Queue processed");
            _notificationService.NotifySuccess("Sync successful", "Synchronization with the server was successful.");
            await QueueProcessed();
        }

        public void OnApiProcessingError()
        {
            ApiHasProcessingError = true;
        }

        public void ResolveProcessingError()
        {
            ApiHasProcessingError = false;
        }
    }
}