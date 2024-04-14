using Fluxor;
using Moq;
using ProjectHermes.Xipona.Frontend.Redux.Manufacturers.States;

namespace ProjectHermes.Xipona.Frontend.Redux.TestKit.Manufacturers.States;

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