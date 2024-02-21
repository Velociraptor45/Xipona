using Moq;
using Moq.Contrib.InOrder.Extensions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;

namespace ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Shared.Ports;

public class ShoppingListNotificationServiceMock : Mock<IShoppingListNotificationService>
{
    public ShoppingListNotificationServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupNotifyWarning(string title, string message)
    {
        this.SetupInOrder(x => x.NotifyWarning(title, message));
    }

    public void SetupNotifyWarningContains(string title, string messagePart)
    {
        this.SetupInOrder(x => x.NotifyWarning(title, It.Is<string>(s => s.Contains(messagePart))));
    }

    public void SetupNotifySuccess(string title, string message)
    {
        this.SetupInOrder(x => x.NotifySuccess(title, message));
    }

    public void SetupNotifySuccess(string message, double? duration = 2)
    {
        this.SetupInOrder(x => x.NotifySuccess(message, duration));
    }

    public void SetupNotifyError(string title, string message)
    {
        this.SetupInOrder(x => x.NotifyError(title, message));
    }

    public void SetupNotifyErrorAsyncContaining(string title, string messagePart)
    {
        this.SetupInOrder(x => x.NotifyError(title, It.Is<string>(msg => msg.Contains(messagePart))));
    }
}