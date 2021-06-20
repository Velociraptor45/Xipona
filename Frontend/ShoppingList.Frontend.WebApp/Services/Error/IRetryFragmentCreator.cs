using Microsoft.AspNetCore.Components;
using System;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Services.Error
{
    public interface IRetryFragmentCreator
    {
        RenderFragment CreateRetryFragment(Action action);
    }
}