using Fluxor;
using Moq;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Stores.States;

public class StoreStateMock : Mock<IState<StoreState>>
{
    public StoreStateMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupValue(StoreState returnValue)
    {
        SetupGet(s => s.Value).Returns(returnValue);
    }
}