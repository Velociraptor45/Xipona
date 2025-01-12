using Moq;
using Moq.Contrib.InOrder.Extensions;

namespace ProjectHermes.Xipona.Frontend.TestTools;
public class TimerMock : Mock<ITimer>
{
    public TimerMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupDispose()
    {
        this.SetupInOrder(x => x.Dispose());
    }

    public void VerifyDispose(Func<Times> times)
    {
        this.Verify(x => x.Dispose(), times);
    }
}
