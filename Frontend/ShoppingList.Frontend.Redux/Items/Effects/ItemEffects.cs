using Fluxor;
using Microsoft.AspNetCore.Components;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions.Editor;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions.Search;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Constants;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.Effects;

public class ItemEffects
{
    private readonly IApiClient _client;
    private readonly NavigationManager _navigationManager;
    private readonly IState<ItemState> _state;

    public ItemEffects(IApiClient client, NavigationManager navigationManager, IState<ItemState> state)
    {
        _client = client;
        _navigationManager = navigationManager;
        _state = state;
    }

    [EffectMethod(typeof(EnterItemSearchPageAction))]
    public Task HandleEnterItemSearchPageAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new LoadQuantityTypesAction());
        dispatcher.Dispatch(new LoadQuantityTypesInPacketAction());
        dispatcher.Dispatch(new LoadActiveStoresAction());

        return Task.CompletedTask;
    }

    [EffectMethod]
    public async Task HandleSearchItemsAction(SearchItemsAction action, IDispatcher dispatcher)
    {
        if (string.IsNullOrWhiteSpace(action.SearchInput))
        {
            dispatcher.Dispatch(new SearchItemsFinishedAction(new List<ItemSearchResult>()));
            return;
        }

        dispatcher.Dispatch(new SearchItemsStartedAction());

        var result = await _client.SearchItemsAsync(action.SearchInput);

        var finishAction = new SearchItemsFinishedAction(result.ToList());
        dispatcher.Dispatch(finishAction);
    }

    [EffectMethod]
    public Task HandleEditItemAction(EditItemAction action, IDispatcher dispatcher)
    {
        _navigationManager.NavigateTo($"{PageRoutes.Items}/{action.Id}");
        return Task.CompletedTask;
    }

    [EffectMethod(typeof(LoadQuantityTypesAction))]
    public async Task HandleLoadQuantityTypesAction(IDispatcher dispatcher)
    {
        if (_state.Value.QuantityTypes.Any())
            return;

        var quantityTypes = await _client.GetAllQuantityTypesAsync();
        dispatcher.Dispatch(new LoadQuantityTypesFinishedAction(quantityTypes.ToList()));
    }

    [EffectMethod(typeof(LoadQuantityTypesInPacketAction))]
    public async Task HandleLoadQuantityTypesInPacketAction(IDispatcher dispatcher)
    {
        if (_state.Value.QuantityTypesInPacket.Any())
            return;

        var quantityTypes = await _client.GetAllQuantityTypesInPacketAsync();
        dispatcher.Dispatch(new LoadQuantityTypesInPacketFinishedAction(quantityTypes.ToList()));
    }

    [EffectMethod(typeof(LoadActiveStoresAction))]
    public async Task HandleLoadActiveStoresAction(IDispatcher dispatcher)
    {
        if (_state.Value.Stores.Stores.Any())
            return;

        var stores = await _client.GetAllActiveStoresForItemAsync();
        dispatcher.Dispatch(new LoadActiveStoresFinishedAction(new ActiveStores(stores.ToList())));
    }
}