using Moq;
using Moq.Contrib.InOrder.Extensions;

namespace ProjectHermes.Xipona.Frontend.TestTools;

public class TimeProviderMock : Mock<TimeProvider>
{
    public TimerCallback? CapturedCallback { get; private set; }

    public TimeProviderMock() : base(MockBehavior.Strict)
    {
    }

    public void SetupCreateTimer(TimeSpan delay, ITimer returnValue)
    {
        this.SetupInOrder(x => x.CreateTimer(It.IsAny<TimerCallback>(), null, delay, Timeout.InfiniteTimeSpan))
            .Callback<TimerCallback, object, TimeSpan, TimeSpan>((callback, _, _, _) => CapturedCallback = callback)
            .Returns(returnValue);
    }
}
