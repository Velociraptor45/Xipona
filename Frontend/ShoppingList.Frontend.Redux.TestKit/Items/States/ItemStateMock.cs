using Fluxor;
using Moq;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Items.States;

public class ItemStateMock : Mock<IState<ItemState>>
{
    public ItemStateMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupValue(ItemState returnValue)
    {
        Setup(m => m.Value).Returns(returnValue);
    }
}