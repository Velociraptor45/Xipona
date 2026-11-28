using Moq;
using Moq.Contrib.InOrder;
using Moq.Contrib.InOrder.Extensions;

namespace Xipona.Frontend.TestTools;
public class TimerMock : Mock<ITimer>
{
    public TimerMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupDispose(IQueueComponent component)
    {
        this.SetupInOrder(x => x.Dispose(), component);
    }
}
