namespace ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports
{
    public interface IShoppingListNotificationService
    {
        void Notify(string title, string message);

        void NotifySuccess(string title, string message);

        void NotifySuccess(string message, double? duration = 2);

        void NotifyWarning(string title, string message);

        void NotifyError(string title, string message);
    }
}