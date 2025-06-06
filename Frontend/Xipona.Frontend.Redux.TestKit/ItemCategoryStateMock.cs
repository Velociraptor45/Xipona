using Fluxor;
using Moq;
using Xipona.Frontend.Redux.ItemCategories.States;

namespace Xipona.Frontend.Redux.TestKit;

public class ItemCategoryStateMock : Mock<IState<ItemCategoryState>>
{
    public ItemCategoryStateMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupValue(ItemCategoryState returnValue)
    {
        Setup(x => x.Value).Returns(returnValue);
    }
}