using Microsoft.AspNetCore.Components;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Error;
using System;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Items.Models
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