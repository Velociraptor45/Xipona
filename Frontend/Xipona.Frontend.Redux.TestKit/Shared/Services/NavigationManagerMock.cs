using Microsoft.AspNetCore.Components;
using Moq;

namespace Xipona.Frontend.Redux.TestKit.Shared.Services;

public class NavigationManagerMock : Mock<NavigationManager>
{
    public NavigationManagerMock(MockBehavior behavior) : base(behavior)
    {
    }

    // NavigateTo can't be mocked because it's not virtual. Don't try it.
}