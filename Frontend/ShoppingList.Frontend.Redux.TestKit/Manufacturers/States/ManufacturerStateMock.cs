using Fluxor;
using Moq;
using ProjectHermes.ShoppingList.Frontend.Redux.Manufacturers.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Manufacturers.States;

public class ManufacturerStateMock : Mock<IState<ManufacturerState>>
{
    public ManufacturerStateMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupValue(ManufacturerState returnValue)
    {
        Setup(x => x.Value).Returns(returnValue);
    }
}