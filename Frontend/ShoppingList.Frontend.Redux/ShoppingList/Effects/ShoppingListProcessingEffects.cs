using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions.Processing;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Effects;

public class ShoppingListProcessingEffects
{
    private readonly IState<ShoppingListState> _state;
    private readonly IShoppingListNotificationService _notificationService;

    public ShoppingListProcessingEffects(IState<ShoppingListState> state,
        IShoppingListNotificationService notificationService)
    {
        _state = state;
        _notificationService = notificationService;
    }

    [EffectMethod]
    public async Task HandleApiRequestProcessingErrorOccurredAction(ApiRequestProcessingErrorOccurredAction action,
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

        await _notificationService.NotifyWarningAsync("Request failed", message);
    }

    [EffectMethod(typeof(ApiConnectionDiedAction))]
    public async Task HandleApiConnectionDiedAction(IDispatcher dispatcher)
    {
        await _notificationService.NotifyWarningAsync("Connection interrupted", "Connection to the server was interrupted.");
    }

    [EffectMethod(typeof(QueueProcessedAction))]
    public async Task HandleQueueProcessedAction(IDispatcher dispatcher)
    {
        await _notificationService.NotifySuccessAsync("Sync completed", "Synchronization with the server completed.");
        dispatcher.Dispatch(new ReloadCurrentShoppingListAction());
    }

    [EffectMethod(typeof(ReloadAfterErrorAction))]
    public Task HandleReloadAfterErrorAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new SelectedStoreChangedAction(_state.Value.SelectedStoreId));
        return Task.CompletedTask;
    }
}