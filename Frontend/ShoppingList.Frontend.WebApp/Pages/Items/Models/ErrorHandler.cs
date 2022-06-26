using Microsoft.AspNetCore.Components;
using ProjectHermes.ShoppingList.Frontend.WebApp.Services.Error;
using System;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Items.Models
{
    public class ErrorHandler : IAsyncRetryFragmentCreator
    {
        private readonly Func<Func<Task>, string, RenderFragment> _createAsyncRenderFragment;

        public ErrorHandler(Func<Func<Task>, string, RenderFragment> createAsyncRenderFragment)
        {
            _createAsyncRenderFragment = createAsyncRenderFragment;
        }

        public RenderFragment CreateAsyncRetryFragment(Func<Task> func)
        {
            return _createAsyncRenderFragment(func, "Retry");
        }
    }
}