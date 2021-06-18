using Microsoft.AspNetCore.Components;
using ProjectHermes.ShoppingList.Frontend.Models.Common.Error;
using System;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.Models.Items
{
    public class ItemsErrorHandler : IAsyncRetryFragmentCreator
    {
        private readonly Func<Func<Task>, string, RenderFragment> createAsyncRenderFragment;

        public ItemsErrorHandler(Func<Func<Task>, string, RenderFragment> createAsyncRenderFragment)
        {
            this.createAsyncRenderFragment = createAsyncRenderFragment;
        }

        public RenderFragment CreateAsyncRetryFragment(Func<Task> func)
        {
            return createAsyncRenderFragment(func, "Retry");
        }
    }
}