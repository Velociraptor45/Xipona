using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Frontend.Models.Index
{
    public class ErrorReceiver
    {
        private readonly List<string> stack = new List<string>();
        private readonly Func<Action<object[]>, object[], string, RenderFragment> createRenderFragment;

        public Action StateChanged { get; set; }
        public IReadOnlyCollection<string> Stack => stack.AsReadOnly();
        public bool IsDebug { get; private set; }

        public ErrorReceiver(bool isDebug,
            Func<Action<object[]>, object[], string, RenderFragment> CreateRenderFragment)
        {
            IsDebug = isDebug;
            createRenderFragment = CreateRenderFragment;
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
    }
}