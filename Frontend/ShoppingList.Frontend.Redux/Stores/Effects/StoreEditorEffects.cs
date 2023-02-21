using Fluxor;
using Microsoft.AspNetCore.Components;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Constants;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.Actions.Editor;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Stores.Effects;

public class StoreEditorEffects
{
    private readonly IApiClient _client;
    private readonly IState<StoreState> _state;
    private readonly NavigationManager _navigationManager;

    public StoreEditorEffects(IApiClient client, IState<StoreState> state, NavigationManager navigationManager)
    {
        _client = client;
        _state = state;
        _navigationManager = navigationManager;
    }

    [EffectMethod(typeof(LeaveStoreEditorAction))]
    public Task HandleLeaveStoreEditorAction(IDispatcher dispatcher)
    {
        _navigationManager.NavigateTo(PageRoutes.Stores);
        return Task.CompletedTask;
    }

    [EffectMethod]
    public async Task HandleLoadStoreForEditing(LoadStoreForEditingAction action, IDispatcher dispatcher)
    {
        var result = await _client.GetStoreByIdAsync(action.StoreId);
        dispatcher.Dispatch(new LoadStoreForEditingFinishedAction(result));
    }

    [EffectMethod(typeof(SaveStoreAction))]
    public async Task HandleSaveStoreAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new SaveStoreStartedAction());

        var store = _state.Value.Editor.Store;
        if (store is null)
            return;

        if (store.Id == Guid.Empty)
            await _client.CreateStoreAsync(store);
        else
            await _client.ModifyStoreAsync(store);

        dispatcher.Dispatch(new SaveStoreFinishedAction());
        dispatcher.Dispatch(new LeaveStoreEditorAction());
    }
}