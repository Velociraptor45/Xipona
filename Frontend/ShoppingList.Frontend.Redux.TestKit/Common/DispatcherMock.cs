using Fluxor;
using Moq;

namespace ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;

public class DispatcherMock : Mock<IDispatcher>
{
    public DispatcherMock(MockBehavior behavior) : base(behavior)
    {
    }
}