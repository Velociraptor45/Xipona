using Moq;
using Moq.Contrib.InOrder;
using Moq.Contrib.InOrder.Extensions;
using Xipona.Frontend.Redux.Shared.Ports;
using Xipona.Frontend.Redux.Shared.Ports.Requests;
using Xipona.Frontend.TestTools.Extensions;

namespace Xipona.Frontend.Redux.TestKit.Shared.Ports;

public class CommandQueueMock : Mock<ICommandQueue>
{
    public CommandQueueMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupEnqueue(IApiRequest request, IQueueComponent component)
    {
        this.SetupInOrder(m => m.Enqueue(It.Is<IApiRequest>(r => r.IsRequestEquivalentTo(request))), component)
            .Returns(Task.CompletedTask);
    }

    public void SetupEnqueue(Func<IApiRequest, bool> comparison, IQueueComponent component)
    {
        this.SetupInOrder(m => m.Enqueue(It.Is<IApiRequest>(r => comparison(r))), component)
            .Returns(Task.CompletedTask);
    }

    public void VerifyEnqueue(IApiRequest request, Func<Times> times)
    {
        Verify(m => m.Enqueue(It.Is<IApiRequest>(r => r.IsRequestEquivalentTo(request))), times);
    }

    public void VerifyNoEnqueue<TRequest>() where TRequest : IApiRequest
    {
        Verify(m => m.Enqueue(It.Is<IApiRequest>(r => r.GetType() == typeof(TRequest))), Times.Never);
    }
}