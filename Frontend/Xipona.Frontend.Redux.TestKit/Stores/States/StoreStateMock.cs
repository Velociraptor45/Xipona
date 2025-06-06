using Fluxor;
using Moq;
using Xipona.Frontend.Redux.Stores.States;

namespace Xipona.Frontend.Redux.TestKit.Stores.States;

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