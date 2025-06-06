using Fluxor;
using Moq;
using Xipona.Frontend.Redux.Shared.States;

namespace Xipona.Frontend.Redux.TestKit.Shared.States;

public class SharedStateMock : Mock<IState<SharedState>>
{
    public SharedStateMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupValue(SharedState returnValue)
    {
        Setup(m => m.Value).Returns(returnValue);
    }
}
