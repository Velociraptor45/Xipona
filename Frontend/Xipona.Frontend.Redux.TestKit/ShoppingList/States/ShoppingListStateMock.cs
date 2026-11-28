using Fluxor;
using Moq;
using Xipona.Frontend.Redux.ShoppingList.States;

namespace Xipona.Frontend.Redux.TestKit.ShoppingList.States;

public class ShoppingListStateMock : Mock<IState<ShoppingListState>>
{
    public ShoppingListStateMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupValue(ShoppingListState returnValue)
    {
        Setup(m => m.Value).Returns(returnValue);
    }
}