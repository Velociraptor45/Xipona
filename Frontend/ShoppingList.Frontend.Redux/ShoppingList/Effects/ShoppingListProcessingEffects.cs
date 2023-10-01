using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
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

    [EffectMethod(typeof(ApiRequestProcessingErrorOccurredAction))]
    public Task HandleApiRequestProcessingErrorOccurredAction(IDispatcher dispatcher)
    {
        _notificationService.NotifyWarning("Request failed", "Processing the request failed.");
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
    public Task HandleReloadAfterErrorAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new SelectedStoreChangedAction(_state.Value.SelectedStoreId));
        return Task.CompletedTask;
    }
}