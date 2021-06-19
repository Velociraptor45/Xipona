using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Services.Error
{
    public interface IAsyncRetryFragmentCreator
    {
        RenderFragment CreateAsyncRetryFragment(Func<Task> func);
    }
}