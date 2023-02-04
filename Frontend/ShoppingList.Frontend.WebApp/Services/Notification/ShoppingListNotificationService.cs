using AntDesign;
using Microsoft.AspNetCore.Components;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Services.Notification
{
    public class ShoppingListNotificationService : IShoppingListNotificationService
    {
        private readonly NotificationService _notificationService;

        public ShoppingListNotificationService(NotificationService notificationService)
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

        public void NotifyWarning(string title, string message)
        {
            _notificationService.Open(new NotificationConfig
            {
                Message = title,
                Description = message,
                //NotificationType = NotificationType.Warning
            });
        }

        public void NotifyError(string title, string message)
        {
            _notificationService.Open(new NotificationConfig
            {
                Message = title,
                Description = message,
                //NotificationType = NotificationType.Error
            });
        }

        public void NotifyError(string title, string message, RenderFragment button)
        {
            _notificationService.Open(new NotificationConfig
            {
                Message = title,
                Description = message,
                //NotificationType = NotificationType.Error,
                Btn = button
            });
        }
    }
}