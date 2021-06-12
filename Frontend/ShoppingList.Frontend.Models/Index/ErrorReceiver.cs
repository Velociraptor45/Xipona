using Microsoft.AspNetCore.Components;
using ProjectHermes.ShoppingList.Frontend.Models.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Common.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.Models.Index
{
    public class ErrorReceiver : ICommandQueueErrorHandler
    {
        private readonly List<string> stack = new List<string>();
        private readonly Func<Action<object[]>, object[], string, RenderFragment> createRenderFragment;
        private readonly IShoppingListNotificationService notificationService;

        public Action StateChanged { get; set; }
        public Func<Task> QueueProcessed { get; set; }
        public IReadOnlyCollection<string> Stack => stack.AsReadOnly();
        public bool IsDebug { get; private set; }
        public bool ApiHasProcessingError { get; private set; }

        public ErrorReceiver(bool isDebug,
            Func<Action<object[]>, object[], string, RenderFragment> CreateRenderFragment,
            IShoppingListNotificationService notificationService)
        {
            IsDebug = isDebug;
            createRenderFragment = CreateRenderFragment;
            this.notificationService = notificationService;
        }

        public void ToggleDebugState()
        {
            IsDebug = !IsDebug;
        }

        public RenderFragment CreateRetryFragment(Action<object[]> action, object[] args)
        {
            return createRenderFragment(action, args, "Retry");
        }

        public void Log(string content)
        {
            stack.Add(content);
            Console.WriteLine(content);
            StateChanged();
        }

        public void OnConnectionFailed()
        {
            Log("Connection failed");
            notificationService.NotifyWarning("Connection interrupted", "Connection to the server was interrupted.");
        }

        public async Task OnQueueProcessedAsync()
        {
            Log("Queue processed");
            notificationService.NotifySuccess("Sync successful", "Synchronization with the server was successful.");
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