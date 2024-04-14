using Microsoft.AspNetCore.Components;
using Moq;

namespace ProjectHermes.Xipona.Frontend.Redux.TestKit.Shared.Services;

public class NavigationManagerMock : Mock<NavigationManager>
{
    public NavigationManagerMock(MockBehavior behavior) : base(behavior)
    {
    }
}