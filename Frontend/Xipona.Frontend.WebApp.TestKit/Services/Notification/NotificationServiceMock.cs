using AntDesign;
using Moq;
using Moq.Contrib.InOrder;
using Moq.Contrib.InOrder.Extensions;

namespace Xipona.Frontend.WebApp.TestKit.Services.Notification;

public class NotificationServiceMock : Mock<INotificationService>
{
    public NotificationServiceMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupOpen(NotificationConfig config, IQueueComponent component)
    {
        this.SetupInOrder(x => x.Open(
            It.Is<NotificationConfig>(cfg =>
                cfg.Message.AsT0 == config.Message.AsT0
                && cfg.Description.AsT0 == config.Description.AsT0
                && cfg.Duration == config.Duration
                && cfg.NotificationType == config.NotificationType
                )), component)
            .ReturnsAsync((NotificationRef)null!);
    }
}