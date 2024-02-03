using Moq;
using Moq.Contrib.InOrder.Extensions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;

namespace ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Shared.Ports;

public class ShoppingListNotificationServiceMock : Mock<IShoppingListNotificationService>
{
    public ShoppingListNotificationServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupNotifyWarningAsync(string title, string message)
    {
        this.SetupInOrder(x => x.NotifyWarningAsync(title, message)).Returns(Task.CompletedTask);
    }

    public void SetupNotifyWarningAsyncContains(string title, string messagePart)
    {
        this.SetupInOrder(x => x.NotifyWarningAsync(title, It.Is<string>(s => s.Contains(messagePart))))
            .Returns(Task.CompletedTask);
    }

    public void SetupNotifySuccessAsync(string title, string message)
    {
        this.SetupInOrder(x => x.NotifySuccessAsync(title, message)).Returns(Task.CompletedTask);
    }

    public void SetupNotifySuccessAsync(string message, double? duration = 2)
    {
        this.SetupInOrder(x => x.NotifySuccessAsync(message, duration)).Returns(Task.CompletedTask);
    }

    public void SetupNotifyErrorAsync(string title, string message)
    {
        this.SetupInOrder(x => x.NotifyErrorAsync(title, message)).Returns(Task.CompletedTask);
    }

    public void SetupNotifyErrorAsyncContaining(string title, string messagePart)
    {
        this.SetupInOrder(x => x.NotifyErrorAsync(title, It.Is<string>(msg => msg.Contains(messagePart))))
            .Returns(Task.CompletedTask);
    }
}