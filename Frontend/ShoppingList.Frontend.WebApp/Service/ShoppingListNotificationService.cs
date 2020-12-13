using AntDesign;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Service
{
    public class ShoppingListNotificationService : IShoppingListNotificationService
    {
        private readonly NotificationService notificationService;

        public ShoppingListNotificationService(NotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        public void Notify(string title, string message)
        {
            notificationService.Open(new NotificationConfig
            {
                Message = title,
                Description = message
            });
        }

        public void NotifySuccess(string title, string message)
        {
            notificationService.Open(new NotificationConfig
            {
                Message = title,
                Description = message,
                NotificationType = NotificationType.Success
            });
        }

        public void NotifyWarning(string title, string message)
        {
            notificationService.Open(new NotificationConfig
            {
                Message = title,
                Description = message,
                NotificationType = NotificationType.Warning
            });
        }

        public void NotifyError(string title, string message)
        {
            notificationService.Open(new NotificationConfig
            {
                Message = title,
                Description = message,
                NotificationType = NotificationType.Error
            });
        }
    }
}