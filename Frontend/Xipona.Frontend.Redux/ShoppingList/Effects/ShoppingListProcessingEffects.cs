using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports;
using ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.Processing;

namespace ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Effects;

public class ShoppingListProcessingEffects
{
    private readonly IShoppingListNotificationService _notificationService;

    public ShoppingListProcessingEffects(IShoppingListNotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [EffectMethod]
    public Task HandleApiRequestProcessingErrorOccurredAction(ApiRequestProcessingErrorOccurredAction action,
        IDispatcher dispatcher)
    {
        var requestName = action.FailedRequest.GetType().Name;
        var message = requestName switch
        {
            nameof(PutItemInBasketRequest) => $"Putting {action.FailedRequest.ItemName} in basket failed",
            nameof(RemoveItemFromBasketRequest) => $"Removing {action.FailedRequest.ItemName} from basket failed",
            nameof(ChangeItemQuantityOnShoppingListRequest) => $"Changing quantity of {action.FailedRequest.ItemName} failed",
            nameof(AddTemporaryItemToShoppingListRequest) => $"Adding temporary item {action.FailedRequest.ItemName} failed",
            nameof(RemoveItemFromShoppingListRequest) => $"Removing item {action.FailedRequest.ItemName} failed",
            _ => "Processing the request failed."
        };

        _notificationService.NotifyWarning("Request failed", message);
        return Task.CompletedTask;
    }

    [EffectMethod(typeof(ApiConnectionDiedAction))]
    public Task HandleApiConnectionDiedAction(IDispatcher dispatcher)
    {
        _notificationService.NotifyWarning("Connection interrupted", "Connection to the server was interrupted.");
        return Task.CompletedTask;
    }

    [EffectMethod(typeof(QueueProcessedAction))]
    public Task HandleQueueProcessedAction(IDispatcher dispatcher)
    {
        _notificationService.NotifySuccess("Sync completed", "Synchronization with the server completed.");
        dispatcher.Dispatch(new ReloadCurrentShoppingListAction());
        return Task.CompletedTask;
    }

    [EffectMethod(typeof(ReloadAfterErrorAction))]
    public static Task HandleReloadAfterErrorAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new ReloadCurrentShoppingListAction());
        return Task.CompletedTask;
    }
}