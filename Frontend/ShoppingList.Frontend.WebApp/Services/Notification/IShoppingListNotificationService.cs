using Microsoft.AspNetCore.Components;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Services.Notification
{
    public interface IShoppingListNotificationService
    {
        void Notify(string title, string message);

        void NotifySuccess(string title, string message);

        void NotifyWarning(string title, string message);

        void NotifyError(string title, string message);

        void NotifyError(string title, string message, RenderFragment button);
    }
}