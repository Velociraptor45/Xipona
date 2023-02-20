using Microsoft.AspNetCore.Components;
using Moq;
using Moq.Contrib.InOrder.Extensions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Shared.Services;

public class NavigationManagerMock : Mock<NavigationManager>
{
    public NavigationManagerMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupNavigateTo(string uri)
    {
        this.SetupInOrder(m => m.NavigateTo(uri, false, false));
    }
}