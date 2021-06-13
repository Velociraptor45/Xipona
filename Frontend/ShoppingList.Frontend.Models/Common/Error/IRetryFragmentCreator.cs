using Microsoft.AspNetCore.Components;
using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Common.Error
{
    public interface IRetryFragmentCreator
    {
        RenderFragment CreateRetryFragment(Action<object[]> action, object[] args);
    }
}