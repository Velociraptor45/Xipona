using Microsoft.AspNetCore.Components;
using Moq;
using Moq.Contrib.InOrder.Extensions;
using Moq.Protected;
using System.Reflection;
using Xipona.Frontend.TestTools.Extensions;

namespace Xipona.Frontend.Redux.TestKit.Shared.Services;

public class NavigationManagerMock : Mock<NavigationManager>
{
    public NavigationManagerMock(MockBehavior behavior) : base(behavior)
    {
        typeof(NavigationManager)
            .GetField("_isInitialized", BindingFlags.Instance | BindingFlags.NonPublic)!
            .SetValue(Object, true);
    }
    
    public void SetupNavigateTo(string uri)
    {
        this.Protected().Setup("NavigateToCore", [uri, false]);
    }
}