using Fluxor;
using Moq;
using Moq.Contrib.InOrder.Extensions;

namespace ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;

public class DispatcherMock : Mock<IDispatcher>
{
    public DispatcherMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupDispatch<T>(T action)
    {
        this.SetupInOrder(x => x.Dispatch(action));
    }

    public void SetupDispatchAny<T>()
    {
        this.SetupInOrder(x => x.Dispatch(It.IsAny<T>()));
    }
}