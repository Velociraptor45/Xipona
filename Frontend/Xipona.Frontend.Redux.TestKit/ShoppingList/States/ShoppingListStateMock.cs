using Fluxor;
using Moq;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.Redux.TestKit.ShoppingList.States;

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