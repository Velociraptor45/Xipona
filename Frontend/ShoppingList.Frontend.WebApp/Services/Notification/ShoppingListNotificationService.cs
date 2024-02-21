using AntDesign;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Services.Notification
{
    public class ShoppingListNotificationService : IShoppingListNotificationService
    {
        private readonly INotificationService _notificationService;

        public ShoppingListNotificationService(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public void Notify(string title, string message)
        {
            _notificationService.Open(new NotificationConfig
            {
                Message = title,
                Description = message
            });
        }

        public void NotifySuccess(string title, string message)
        {
            _notificationService.Open(new NotificationConfig
            {
                Message = title,
                Description = message,
                NotificationType = NotificationType.Success
            });
        }

        public void NotifySuccess(string message, double? duration = 2D)
        {
            _notificationService.Open(new NotificationConfig
            {
                Description = message,
                NotificationType = NotificationType.Success,
                Duration = duration
            });
        }

        public void NotifyWarning(string title, string message)
        {
            _notificationService.Open(new NotificationConfig
            {
                Message = title,
                Description = message,
                NotificationType = NotificationType.Warning
            });
        }

        public void NotifyError(string title, string message)
        {
            _notificationService.Open(new NotificationConfig
            {
                Message = title,
                Description = message,
                NotificationType = NotificationType.Error
            });
        }
    }
}