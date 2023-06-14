using Moq;
using Moq.Contrib.InOrder.Extensions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests;
using ProjectHermes.ShoppingList.Frontend.TestTools.Extensions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Shared.Ports;

public class CommandQueueMock : Mock<ICommandQueue>
{
    public CommandQueueMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupEnqueue(IApiRequest request)
    {
        this.SetupInOrder(m =>
                m.Enqueue(It.Is<IApiRequest>(r => r.IsRequestEquivalentTo(request))))
            .Returns(Task.CompletedTask);
    }

    public void SetupEnqueue(Func<IApiRequest, bool> comparison)
    {
        this.SetupInOrder(m =>
                m.Enqueue(It.Is<IApiRequest>(r => comparison(r))))
            .Returns(Task.CompletedTask);
    }

    public void VerifyEnqueue(IApiRequest request, Func<Times> times)
    {
        Verify(m =>
                m.Enqueue(It.Is<IApiRequest>(r => r.IsRequestEquivalentTo(request))),
                times);
    }

    public void VerifyNoEnqueue<TRequest>() where TRequest : IApiRequest
    {
        Verify(m =>
                m.Enqueue(It.Is<IApiRequest>(r => r.GetType() == typeof(TRequest))),
                Times.Never);
    }
}