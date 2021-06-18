using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.Models.Common.Error
{
    public interface IAsyncRetryFragmentCreator
    {
        RenderFragment CreateAsyncRetryFragment(Func<Task> func);
    }
}