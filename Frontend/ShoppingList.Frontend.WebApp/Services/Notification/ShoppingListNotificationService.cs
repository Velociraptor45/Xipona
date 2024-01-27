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

        public async Task NotifyAsync(string title, string message)
        {
            await _notificationService.Open(new NotificationConfig
            {
                Message = title,
                Description = message
            });
        }

        public async Task NotifySuccessAsync(string title, string message)
        {
            await _notificationService.Open(new NotificationConfig
            {
                Message = title,
                Description = message,
                NotificationType = NotificationType.Success
            });
        }

        public async Task NotifySuccessAsync(string message, double? duration = 2)
        {
            await _notificationService.Open(new NotificationConfig
            {
                Description = message,
                NotificationType = NotificationType.Success,
                Duration = duration
            });
        }

        public async Task NotifyWarningAsync(string title, string message)
        {
            await _notificationService.Open(new NotificationConfig
            {
                Message = title,
                Description = message,
                NotificationType = NotificationType.Warning
            });
        }

        public async Task NotifyErrorAsync(string title, string message)
        {
            await _notificationService.Open(new NotificationConfig
            {
                Message = title,
                Description = message,
                NotificationType = NotificationType.Error
            });
        }
    }
}