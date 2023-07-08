using Moq;
using Moq.Contrib.InOrder.Extensions;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;

namespace ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Shared.Ports;

public class ShoppingListNotificationServiceMock : Mock<IShoppingListNotificationService>
{
    public ShoppingListNotificationServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupNotifySuccess(string title, string message)
    {
        this.SetupInOrder(x => x.NotifySuccess(title, message));
    }

    public void SetupNotifySuccess(string message, double? duration = 2)
    {
        this.SetupInOrder(x => x.NotifySuccess(message, duration));
    }
}