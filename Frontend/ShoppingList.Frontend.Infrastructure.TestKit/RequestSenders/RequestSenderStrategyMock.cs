using Moq;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.RequestSenders;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests;

namespace ShoppingList.Frontend.Infrastructure.TestKit.RequestSenders;

public class RequestSenderStrategyMock : Mock<IRequestSenderStrategy>
{
    public RequestSenderStrategyMock(MockBehavior mockBehavior) : base(mockBehavior)
    {
    }

    public void SetupSendAsync(IApiRequest request)
    {
        Setup(x => x.SendAsync(request)).Returns(Task.CompletedTask);
    }

    public void SetupSendAsyncThrowing(IApiRequest request, Exception ex)
    {
        Setup(x => x.SendAsync(request)).ThrowsAsync(ex);
    }
}