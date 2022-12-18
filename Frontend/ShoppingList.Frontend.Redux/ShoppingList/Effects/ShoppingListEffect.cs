using Fluxor;
using ShoppingList.Frontend.Redux.Shared.Ports;
using ShoppingList.Frontend.Redux.ShoppingList.Actions;
using ShoppingList.Frontend.Redux.ShoppingList.States;

namespace ShoppingList.Frontend.Redux.ShoppingList.Effects;

public class ShoppingListEffects
{
    private readonly IApiClient _client;
    private readonly IState<ShoppingListState> _state;

    public ShoppingListEffects(IApiClient client, IState<ShoppingListState> state)
    {
        _client = client;
        _state = state;
    }

    [EffectMethod(typeof(LoadQuantityTypesAction))]
    public async Task HandleLoadQuantityTypesAction(IDispatcher dispatcher)
    {
        var quantityTypes = await _client.GetAllQuantityTypesAsync();
        dispatcher.Dispatch(new LoadQuantityTypesFinishedAction(quantityTypes));
    }

    [EffectMethod(typeof(LoadQuantityTypesInPacketAction))]
    public async Task HandleLoadQuantityTypesInPacketAction(IDispatcher dispatcher)
    {
        var quantityTypes = await _client.GetAllQuantityTypesInPacketAsync();
        dispatcher.Dispatch(new LoadQuantityTypesInPacketFinishedAction(quantityTypes));
    }

    [EffectMethod(typeof(LoadAllActiveStoresAction))]
    public async Task HandleLoadAllActiveStoresAction(IDispatcher dispatcher)
    {
        var stores = await _client.GetAllActiveStoresForShoppingListAsync();
        var finishAction = new LoadAllActiveStoresFinishedAction(new AllActiveStores(stores.ToList()));
        dispatcher.Dispatch(finishAction);

        if (stores.Any())
            dispatcher.Dispatch(new SelectedStoreChangedAction(stores.First().Id));
    }
}