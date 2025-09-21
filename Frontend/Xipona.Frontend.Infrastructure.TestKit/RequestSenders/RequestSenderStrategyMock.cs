using Moq;
using Moq.Contrib.InOrder;
using Moq.Contrib.InOrder.Extensions;
using Xipona.Frontend.Infrastructure.RequestSenders;
using Xipona.Frontend.Redux.Shared.Ports.Requests;

namespace Xipona.Frontend.Infrastructure.TestKit.RequestSenders;

public class RequestSenderStrategyMock : Mock<IRequestSenderStrategy>
{
    public RequestSenderStrategyMock(MockBehavior mockBehavior) : base(mockBehavior)
    {
    }

    public void SetupSendAsync(IApiRequest request, IQueueComponent component)
    {
        this.SetupInOrder(x => x.SendAsync(request), component).Returns(Task.CompletedTask);
    }

    public void SetupSendAsyncThrowing(IApiRequest request, Exception ex, IQueueComponent component)
    {
        this.SetupInOrder(x => x.SendAsync(request), component).ThrowsAsync(ex);
    }
}