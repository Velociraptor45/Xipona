using Fluxor;
using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;
using ShoppingList.Frontend.Redux.Shared.Ports;
using ShoppingList.Frontend.Redux.Shared.Ports.Requests.Items;
using ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists;
using ShoppingList.Frontend.Redux.Shared.States;
using ShoppingList.Frontend.Redux.ShoppingList.Actions;
using ShoppingList.Frontend.Redux.ShoppingList.Actions.PriceUpdater;
using ShoppingList.Frontend.Redux.ShoppingList.Actions.Summary;
using ShoppingList.Frontend.Redux.ShoppingList.Actions.TemporaryItemCreator;
using ShoppingList.Frontend.Redux.ShoppingList.States;
using ShoppingListItem = ShoppingList.Frontend.Redux.ShoppingList.States.ShoppingListItem;

namespace ShoppingList.Frontend.Redux.ShoppingList.Effects;

public class ShoppingListEffects
{
    private readonly IApiClient _client;
    private readonly ICommandQueue _commandQueue;
    private readonly IState<ShoppingListState> _state;

    public ShoppingListEffects(IApiClient client, ICommandQueue commandQueue, IState<ShoppingListState> state)
    {
        _client = client;
        _commandQueue = commandQueue;
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

    [EffectMethod]
    public async Task HandleSelectedStoreChangedAction(SelectedStoreChangedAction action, IDispatcher dispatcher)
    {
        var shoppingList = await _client.GetActiveShoppingListByStoreIdAsyncNew(action.StoreId);

        dispatcher.Dispatch(new LoadShoppingListFinishedAction(shoppingList));
    }

    [EffectMethod(typeof(SaveTemporaryItemAction))]
    public async Task HandleSaveTemporaryItemAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new SaveTemporaryItemStartedAction());

        // todo merge these requests (#333)
        var quantityType = new QuantityType(0, "", 1, "€", "x", 1);
        var quantityTypeInPacket = new QuantityTypeInPacket(0, "", "");
        var offlineId = ShoppingListItemId.FromOfflineId(Guid.NewGuid());
        var item = new ShoppingListItem(
            offlineId,
            TypeId: null,
            _state.Value.TemporaryItemCreator.ItemName,
            IsTemporary: true,
            _state.Value.TemporaryItemCreator.Price,
            quantityType,
            QuantityInPacket: 1,
            quantityTypeInPacket,
            ItemCategory: "",
            Manufacturer: "",
            IsInBasket: false,
            Quantity: 1,
            false);

        var createRequest = new CreateTemporaryItemRequest(
            Guid.NewGuid(),
            item.Id.OfflineId!.Value,
            item.Name,
            _state.Value.SelectedStoreId,
            item.PricePerQuantity,
            _state.Value.TemporaryItemCreator.Section!.Id);
        var addRequest = new AddItemToShoppingListRequest(
            Guid.NewGuid(),
            _state.Value.ShoppingList!.Id,
            item.Id,
            1,
            null);

        await _commandQueue.Enqueue(createRequest);
        await _commandQueue.Enqueue(addRequest);

        dispatcher.Dispatch(new SaveTemporaryItemFinishedAction(item, _state.Value.TemporaryItemCreator.Section));
        dispatcher.Dispatch(new CloseTemporaryItemCreatorAction());
    }

    [EffectMethod(typeof(SavePriceUpdateAction))]
    public async Task HandleSavePriceUpdateAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new SavePriceUpdateStartedAction());

        var typeId = _state.Value.PriceUpdate.UpdatePriceForAllTypes ? null : _state.Value.PriceUpdate.Item!.TypeId;

        var request = new UpdateItemPriceRequest(
            _state.Value.PriceUpdate.Item!.Id.ActualId!.Value,
            typeId,
            _state.Value.SelectedStoreId,
            _state.Value.PriceUpdate.Price);

        await _client.UpdateItemPriceAsync(request);

        dispatcher.Dispatch(new SavePriceUpdateFinishedAction(
            _state.Value.PriceUpdate.Item.Id.ActualId!.Value,
            _state.Value.PriceUpdate.Item.TypeId,
            _state.Value.PriceUpdate.Price));

        dispatcher.Dispatch(new ClosePriceUpdaterAction());
    }

    [EffectMethod(typeof(FinishShoppingListAction))]
    public async Task HandleFinishShoppingListAction(IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new FinishShoppingListStartedAction());

        var finishedAt = new DateTimeOffset(_state.Value.Summary.FinishedAt.ToUniversalTime(),
            TimeSpan.Zero);

        var request = new FinishListRequest(
            Guid.NewGuid(),
            _state.Value.ShoppingList!.Id,
            finishedAt);
        await _client.FinishListAsync(request);

        dispatcher.Dispatch(new FinishShoppingListFinishedAction());
        dispatcher.Dispatch(new SelectedStoreChangedAction(_state.Value.SelectedStoreId));
    }
}