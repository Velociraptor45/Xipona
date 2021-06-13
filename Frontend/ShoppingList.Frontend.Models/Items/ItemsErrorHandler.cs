using Microsoft.AspNetCore.Components;
using ProjectHermes.ShoppingList.Frontend.Models.Common.Error;
using System;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.Models.Items
{
    public class ItemsErrorHandler : IAsyncRetryFragmentCreator
    {
        private readonly Func<Func<object[], Task>, object[], string, RenderFragment> createAsyncRenderFragment;

        public ItemsErrorHandler(Func<Func<object[], Task>, object[], string, RenderFragment> createAsyncRenderFragment)
        {
            this.createAsyncRenderFragment = createAsyncRenderFragment;
        }

        public RenderFragment CreateAsyncRetryFragment(Func<object[], Task> func, object[] args)
        {
            return createAsyncRenderFragment(func, args, "Retry");
        }
    }
}