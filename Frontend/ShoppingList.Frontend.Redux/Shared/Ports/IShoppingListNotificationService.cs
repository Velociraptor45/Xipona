namespace ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports
{
    public interface IShoppingListNotificationService
    {
        Task NotifyAsync(string title, string message);

        Task NotifySuccessAsync(string title, string message);

        Task NotifySuccessAsync(string message, double? duration = 2);

        Task NotifyWarningAsync(string title, string message);

        Task NotifyErrorAsync(string title, string message);
    }
}