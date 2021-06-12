using AntDesign;
using Microsoft.AspNetCore.Components;
using ProjectHermes.ShoppingList.Frontend.Models.Common.Services;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Services
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
                //NotificationType = NotificationType.Warning
            });
        }

        public void NotifyError(string title, string message)
        {
            notificationService.Open(new NotificationConfig
            {
                Message = title,
                Description = message,
                //NotificationType = NotificationType.Error
            });
        }

        public void NotifyError(string title, string message, RenderFragment button)
        {
            notificationService.Open(new NotificationConfig
            {
                Message = title,
                Description = message,
                //NotificationType = NotificationType.Error,
                Btn = button
            });
        }
    }
}