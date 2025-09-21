using Moq;
using Moq.Contrib.InOrder;
using Moq.Contrib.InOrder.Extensions;
using Xipona.Frontend.Redux.Shared.Ports;

namespace Xipona.Frontend.Redux.TestKit.Shared.Ports;

public class ShoppingListNotificationServiceMock : Mock<IShoppingListNotificationService>
{
    public ShoppingListNotificationServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupNotifyWarning(string title, string message, IQueueComponent component)
    {
        this.SetupInOrder(x => x.NotifyWarning(title, message), component);
    }

    public void SetupNotifyWarningContains(string title, string messagePart, IQueueComponent component)
    {
        this.SetupInOrder(x => x.NotifyWarning(title, It.Is<string>(s => s.Contains(messagePart))), component);
    }

    public void SetupNotifySuccess(string title, string message, IQueueComponent component)
    {
        this.SetupInOrder(x => x.NotifySuccess(title, message), component);
    }

    public void SetupNotifySuccess(string message, double? duration, IQueueComponent component)
    {
        this.SetupInOrder(x => x.NotifySuccess(message, duration), component);
    }

    public void SetupNotifyError(string title, string message, IQueueComponent component)
    {
        this.SetupInOrder(x => x.NotifyError(title, message), component);
    }

    public void SetupNotifyErrorAsyncContaining(string title, string messagePart, IQueueComponent component)
    {
        this.SetupInOrder(x => x.NotifyError(title, It.Is<string>(msg => msg.Contains(messagePart))), component);
    }
}