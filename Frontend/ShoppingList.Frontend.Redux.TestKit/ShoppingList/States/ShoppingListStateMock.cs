using Fluxor;
using Moq;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.TestKit.ShoppingList.States;

public class ShoppingListStateMock : Mock<IState<ShoppingListState>>
{
    public ShoppingListStateMock(MockBehavior behavior) : base(behavior)
    {
    }
}