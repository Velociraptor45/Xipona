using RestEase;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;
public record DisplayApiExceptionNotificationAction(string Title, ApiException Exception);