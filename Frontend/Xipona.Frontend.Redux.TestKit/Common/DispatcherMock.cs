using Fluxor;
using Moq;
using Moq.Contrib.InOrder;
using Moq.Contrib.InOrder.Extensions;
using Xipona.Frontend.TestTools.Extensions;

namespace Xipona.Frontend.Redux.TestKit.Common;

public class DispatcherMock : Mock<IDispatcher>
{
    public DispatcherMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupDispatch<T>(T action, IQueueComponent component)
    {
        this.SetupInOrder(x => x.Dispatch(It.Is<T>(x => x.IsEquivalentTo(action))), component);
    }

    public void SetupDispatchAny<T>(IQueueComponent component)
    {
        this.SetupInOrder(x => x.Dispatch(It.IsAny<T>()), component);
    }
}